using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CRMSystem.API.Common.Routes;
using CRMSystem.API.IntegrationTests.Infrastructure;
using CRMSystem.Application.Common.Authorization;
using CRMSystem.Application.Features.Auth.Contracts;
using CRMSystem.Application.Identity;
using CRMSystem.Domain.Enums;
using CRMSystem.Infrastructure.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CRMSystem.API.IntegrationTests.Auth;

public class RegisterAgentsTests : IntegrationTestBase
{
    private const string RegisterOperatorsRoute = $"{AuthRoutes.Base}{AuthRoutes.Operators}";
    private const string RegisterAdminsRoute = $"{AuthRoutes.Base}{AuthRoutes.Admins}";
    private const string RegisterClientsRoute = $"{AuthRoutes.Base}{AuthRoutes.Clients}";
    private const string LoginRoute = $"{AuthRoutes.Base}{AuthRoutes.Login}";

    private const string ClientEmail = "client@gmail.com";
    private const string ClientPassword = "Client@123";
    private const string SuperAdminEmail = "superadmin@gmail.com";
    private const string SuperAdminPassword = "Admin@123";
    private const string AdminEmail = "admin@gmail.com";
    private const string AdminPassword = "Admin@123";
    private const string OperatorEmail = "operator@gmail.com";
    private const string OperatorPassword = "Operator@123";
    private const string FirstName = "John";
    private const string LastName = "Doe";

    private const string AuthScheme = "Bearer";


    public RegisterAgentsTests(PostgresContainerFixture containerFixture) : base(containerFixture)
    {
    }

    [Fact]
    public async Task RegisterAgent_InvalidRequest_ReturnsBadRequest()
    {
        var token = await LoginAndGetTokenAsync(SuperAdminEmail, SuperAdminPassword);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthScheme, token);

        var response = await Client.PostAsJsonAsync(RegisterOperatorsRoute, new RegisterAgentRequest(
            Email: "bad-email",
            Password: "1",
            FirstName: "",
            LastName: ""
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
    public async Task RegisterOperator_Unauthenticated_ReturnsUnauthorized()
    {
        var response = await Client.PostAsJsonAsync(RegisterOperatorsRoute, new RegisterAgentRequest(
            OperatorEmail, OperatorPassword, FirstName, LastName));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RegisterOperator_AuthenticatedAsClient_ReturnsForbidden()
    {
        await Client.PostAsJsonAsync(RegisterClientsRoute,
            new RegisterClientRequest(ClientEmail, ClientPassword, FirstName, LastName));

        var clientToken = await LoginAndGetTokenAsync(ClientEmail, ClientPassword);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthScheme, clientToken);

        var response = await Client.PostAsJsonAsync(
            RegisterOperatorsRoute, new RegisterAgentRequest(
                OperatorEmail, OperatorPassword, FirstName, LastName));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task RegisterOperator_AuthenticatedAsSuperAdmin_ReturnsCreatedAndAssignsOperatorRole()
    {
        var token = await LoginAndGetTokenAsync(SuperAdminEmail, SuperAdminPassword);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthScheme, token);

        var response = await Client.PostAsJsonAsync(
            RegisterOperatorsRoute, new RegisterAgentRequest(
                OperatorEmail, OperatorPassword, FirstName, LastName));
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var userId = await response.Content.ReadFromJsonAsync<Guid>();
        userId.Should().NotBe(Guid.Empty);

        await WithScopeAsync(async scope =>
        {
            var userManager = scope.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = scope.GetRequiredService<CrmDbContext>();

            var user = await userManager.FindByIdAsync(userId.ToString());
            user.Should().NotBeNull();
            user.Id.Should().Be(userId);

            var roles = await userManager.GetRolesAsync(user);
            roles.Should().Contain(Roles.Operator);

            var agent = await dbContext.Agents.SingleOrDefaultAsync(a => a.ActorId == user.ActorId);
            agent.Should().NotBeNull();
            agent.Status.Should().Be(AgentStatus.Active);
        });
    }

    [Fact]
    public async Task RegisterAdmin_AuthenticatedAsSuperAdmin_ReturnsCreatedAndAssignsAdminRole()
    {
        var token = await LoginAndGetTokenAsync(SuperAdminEmail, SuperAdminPassword);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthScheme, token);

        var response = await Client.PostAsJsonAsync(RegisterAdminsRoute, new RegisterAgentRequest(
            AdminEmail, AdminPassword, FirstName, LastName));

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var userId = await response.Content.ReadFromJsonAsync<Guid>();
        userId.Should().NotBe(Guid.Empty);

        await WithScopeAsync(async scope =>
        {
            var userManager = scope.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = scope.GetRequiredService<CrmDbContext>();

            var user = await userManager.FindByIdAsync(userId.ToString());
            user.Should().NotBeNull();
            user.Id.Should().Be(userId);

            var roles = await userManager.GetRolesAsync(user);
            roles.Should().Contain(Roles.Admin);

            var agent = await dbContext.Agents.SingleOrDefaultAsync(a => a.ActorId == user.ActorId);
            agent.Should().NotBeNull();
            agent.Status.Should().Be(AgentStatus.Active);
        });
    }

    [Fact]
    public async Task RegisterAdmin_AuthenticatedAsAdmin_ReturnsForbidden()
    {
        var superToken = await LoginAndGetTokenAsync(SuperAdminEmail, SuperAdminPassword);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthScheme, superToken);

        var createAdmin = await Client.PostAsJsonAsync(
            RegisterAdminsRoute, new RegisterAgentRequest(
                AdminEmail, AdminPassword, FirstName, LastName));
        createAdmin.StatusCode.Should().Be(HttpStatusCode.Created);

        var adminToken = await LoginAndGetTokenAsync(AdminEmail, AdminPassword);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthScheme, adminToken);

        var response = await Client.PostAsJsonAsync(
            RegisterAdminsRoute,
            new RegisterAgentRequest("admin2@test.com", "Password123!", "Admin", "Admin"));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<string> LoginAndGetTokenAsync(string email, string password)
    {
        Client.DefaultRequestHeaders.Authorization = null;

        var resp = await Client.PostAsJsonAsync(
            LoginRoute,
            new LoginRequest(email, password));

        var payload = await resp.Content.ReadFromJsonAsync<LoginResponse>();
        payload.Should().NotBeNull();

        return payload.Token;
    }
}