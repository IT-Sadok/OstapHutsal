using Testcontainers.PostgreSql;

namespace CRMSystem.API.IntegrationTests.Infrastructure;

public sealed class PostgresContainerFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; } =
        new PostgreSqlBuilder("postgres:18.1")
            .WithDatabase("crm_tests")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    public Task InitializeAsync() => Container.StartAsync();

    public Task DisposeAsync() => Container.StopAsync();
}