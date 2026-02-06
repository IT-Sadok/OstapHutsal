using Npgsql;
using Respawn;
using Respawn.Graph;

namespace CRMSystem.API.IntegrationTests.Infrastructure;

public sealed class DatabaseReset
{
    private readonly string _connectionString;
    private Respawner? _respawner;

    public DatabaseReset(string connectionString) => _connectionString = connectionString;

    public async Task InitializeAsync()
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        _respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            TablesToIgnore =
            [
                new Table("__EFMigrationsHistory"),
                new Table("AspNetRoles"),
                new Table("ticket_categories"),
                new Table("priorities"),
                new Table("communication_channels")
            ]
        });
    }

    public async Task ResetAsync()
    {
        if (_respawner is null) throw new InvalidOperationException("Call InitializeAsync() first.");

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await _respawner.ResetAsync(connection);
    }
}