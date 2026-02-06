using System.Net;
using System.Net.Http.Json;
using CRMSystem.API.Common.ErrorMapping;
using CRMSystem.API.Common.Routes;
using CRMSystem.API.IntegrationTests.Infrastructure;
using CRMSystem.Application.Common.Authorization;
using CRMSystem.Application.Features.Auth;
using CRMSystem.Application.Features.Auth.Contracts;
using CRMSystem.Application.Identity;
using CRMSystem.Infrastructure.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CRMSystem.API.IntegrationTests.Auth;

public class RegisterClientsTests : IntegrationTestBase
{
    private const string RegisterClientsRoute = $"{AuthRoutes.Base}{AuthRoutes.Clients}";

    private const string ClientEmail = "client@gmail.com";
    private const string ClientPassword = "Client@123";
    private const string FirstName = "John";
    private const string LastName = "Doe";

    public RegisterClientsTests(PostgresContainerFixture containerFixture) : base(containerFixture)
    {
    }

    [Fact]
    public async Task RegisterClient_InvalidRequest_ReturnsBadRequest()
    {
        var response = await Client.PostAsJsonAsync(RegisterClientsRoute, new RegisterClientRequest(
            Email: "bad-email",
            Password: "1",
            FirstName: "",
            LastName: "",
            Phone: null,
            Address: null
        ));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var payload = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        payload.Should().NotBeNull();
        payload.Errors.Should().NotBeEmpty();
        payload.Errors.Keys.Should().Contain([
            "Email",
            "Password",
            "FirstName",
            "LastName"
        ]);
    }

    [Fact]
    public async Task RegisterClient_ValidRequest_ReturnsCreatedAndUserId()
    {
        const string phone = "+380111222333";

        var response = await Client.PostAsJsonAsync(RegisterClientsRoute, new RegisterClientRequest(
            Email: ClientEmail,
            Password: ClientPassword,
            FirstName: FirstName,
            LastName: LastName,
            Phone: phone,
            Address: null
        ));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var userId = await response.Content.ReadFromJsonAsync<Guid>();
        userId.Should().NotBe(Guid.Empty);

        await WithScopeAsync(async scope =>
        {
            var userManager = scope.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = scope.GetRequiredService<CrmDbContext>();

            var user = await userManager.FindByEmailAsync(ClientEmail);
            user.Should().NotBeNull();
            user.Id.Should().Be(userId);

            var roles = await userManager.GetRolesAsync(user);
            roles.Should().Contain(Roles.Client);

            var client = await dbContext.Clients.SingleOrDefaultAsync(c => c.ActorId == user.ActorId);
            client.Should().NotBeNull();
            client.Phone.Should().Be(phone);
        });
    }

    [Fact]
    public async Task RegisterClient_DuplicateEmail_ReturnsConflictWithCode()
    {
        var response = await Client.PostAsJsonAsync(RegisterClientsRoute, new RegisterClientRequest(
            Email: ClientEmail,
            Password: ClientPassword,
            FirstName: FirstName,
            LastName: LastName,
            Phone: null,
            Address: null
        ));
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var conflictResponse = await Client.PostAsJsonAsync(RegisterClientsRoute, new RegisterClientRequest(
            Email: ClientEmail,
            Password: ClientPassword,
            FirstName: FirstName,
            LastName: LastName,
            Phone: null,
            Address: null
        ));
        conflictResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var payload = await ReadProblemAsync(conflictResponse);
        payload.Extensions[ProblemDetailsExtensions.ErrorCode]?.ToString().Should()
            .Be(AuthErrorCodes.EmailAlreadyExists);
    }
}