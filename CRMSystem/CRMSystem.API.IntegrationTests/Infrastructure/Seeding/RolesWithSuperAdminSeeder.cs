using CRMSystem.Infrastructure.Data.Seeding;

namespace CRMSystem.API.IntegrationTests.Infrastructure.Seeding;

public class RolesWithSuperAdminSeeder
{
    public static async Task SeedAsync(IServiceProvider sp)
    {
        await RoleSeeder.SeedRolesAsync(sp);
    }
}