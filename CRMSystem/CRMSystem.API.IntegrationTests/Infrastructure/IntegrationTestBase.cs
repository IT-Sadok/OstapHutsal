using System.Net.Http.Json;
using CRMSystem.API.IntegrationTests.Infrastructure.Seeding;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CRMSystem.API.IntegrationTests.Infrastructure;

[Collection(nameof(PostgresCollection))]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    private DatabaseReset _databaseReset = null!;

    protected readonly PostgresContainerFixture ContainerFixture;
    protected IntegrationTestWebAppFactory WebAppFactory = null!;
    protected HttpClient Client = null!;

    protected IntegrationTestBase(PostgresContainerFixture containerFixture)
    {
        ContainerFixture = containerFixture;
    }

    public async Task InitializeAsync()
    {
        var connectionString = ContainerFixture.Container.GetConnectionString();

        WebAppFactory = new IntegrationTestWebAppFactory(connectionString);
        Client = WebAppFactory.CreateClient();

        _databaseReset = new DatabaseReset(connectionString);
        await _databaseReset.InitializeAsync();

        await ResetAndSeedRolesWithSuperAdminAsync();
    }

    public Task DisposeAsync()
    {
        Client.Dispose();
        WebAppFactory.Dispose();
        return Task.CompletedTask;
    }

    protected async Task ResetAndSeedBaselineAsync()
    {
        await _databaseReset.ResetAsync();

        using var scope = WebAppFactory.Services.CreateScope();
        await BaselineSeed.SeedAsync(scope.ServiceProvider);
    }

    protected async Task ResetAndSeedRolesWithSuperAdminAsync()
    {
        await _databaseReset.ResetAsync();

        using var scope = WebAppFactory.Services.CreateScope();
        await RolesWithSuperAdminSeeder.SeedAsync(scope.ServiceProvider);
    }

    protected async Task<T> WithScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = WebAppFactory.Services.CreateScope();
        return await action(scope.ServiceProvider);
    }

    protected async Task WithScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = WebAppFactory.Services.CreateScope();
        await action(scope.ServiceProvider);
    }

    protected static async Task<ProblemDetails> ReadProblemAsync(HttpResponseMessage response)
    {
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        return problemDetails;
    }
}