using System.Net;
using System.Net.Http.Json;
using CRMSystem.API.Common.ErrorMapping;
using CRMSystem.API.Common.Routes;
using CRMSystem.API.IntegrationTests.Infrastructure;
using CRMSystem.Application.Common.Errors;
using CRMSystem.Application.Features.Auth;
using CRMSystem.Application.Features.Auth.Contracts;
using CRMSystem.Application.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CRMSystem.API.IntegrationTests.Auth;

public sealed class LoginTests : IntegrationTestBase
{
    private const string LoginRoute = $"{AuthRoutes.Base}{AuthRoutes.Login}";
    private const string SuperAdminEmail = "superadmin@gmail.com";
    private const string SuperAdminPassword = "Admin@123";

    public LoginTests(PostgresContainerFixture postgresContainer) : base(postgresContainer)
    {
    }

    [Fact]
    public async Task Login_InvalidRequest_ReturnsBadRequest()
    {
        var response = await Client.PostAsJsonAsync(LoginRoute,
            new LoginRequest("", ""));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var payload = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        payload.Should().NotBeNull();
        payload.Errors.Should().NotBeEmpty();
        payload.Errors.Keys.Should().Contain([
            "Email",
            "Password",
        ]);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        var response = await Client.PostAsJsonAsync(LoginRoute,
            new LoginRequest(SuperAdminEmail, SuperAdminPassword));

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<LoginResponse>();
        payload.Should().NotBeNull();
        payload.Token.Should().NotBeNullOrWhiteSpace();
        payload.Token.Split('.').Length.Should().Be(3);

        await WithScopeAsync(async sp =>
        {
            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync(SuperAdminEmail);
            user.Should().NotBeNull();
            user.AccessFailedCount.Should().Be(0);
        });
    }

    [Fact]
    public async Task Login_UserDoesNotExist_ReturnsNotFound()
    {
        var response =
            await Client.PostAsJsonAsync(LoginRoute,
                new LoginRequest("missing.user@test.com", "Password123!"));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await ReadProblemAsync(response);
        problem.Extensions[ProblemDetailsExtensions.ErrorCode]?.ToString().Should().Be(CommonErrorCodes.NotFound);
    }

    [Fact]
    public async Task Login_InvalidPassword_ReturnsUnauthorizedWithIncrementAccessFailed()
    {
        var beforeAccessFailedCount = await WithScopeAsync(async scope =>
        {
            var userManager = scope.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync(SuperAdminEmail);
            return user!.AccessFailedCount;
        });

        var response =
            await Client.PostAsJsonAsync(LoginRoute,
                new LoginRequest(SuperAdminEmail, "Wrong@123"));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var problem = await ReadProblemAsync(response);
        problem.Extensions[ProblemDetailsExtensions.ErrorCode]?.ToString().Should().Be(AuthErrorCodes.InvalidPassword);

        var afterAccessFailedCount = await WithScopeAsync(async scope =>
        {
            var userManager = scope.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync(SuperAdminEmail);
            return user!.AccessFailedCount;
        });

        afterAccessFailedCount.Should().Be(beforeAccessFailedCount + 1);
    }

    [Fact]
    public async Task Login_UserIsLockedOut_ReturnsUnauthorized()
    {
        await WithScopeAsync(async scope =>
        {
            var userManager = scope.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync(SuperAdminEmail);
            user.Should().NotBeNull();

            var enabled = await userManager.SetLockoutEnabledAsync(user!, true);
            enabled.Succeeded.Should().BeTrue();

            var locked = await userManager.SetLockoutEndDateAsync(user!, DateTimeOffset.UtcNow.AddMinutes(20));
            locked.Succeeded.Should().BeTrue();
        });

        var response = await Client.PostAsJsonAsync(LoginRoute,
            new LoginRequest(SuperAdminEmail, SuperAdminPassword));
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var problem = await ReadProblemAsync(response);
        problem.Extensions[ProblemDetailsExtensions.ErrorCode]?.ToString().Should().Be(AuthErrorCodes.UserLocked);
    }
}