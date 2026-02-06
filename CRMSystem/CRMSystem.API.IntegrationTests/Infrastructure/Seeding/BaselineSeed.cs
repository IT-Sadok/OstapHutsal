using CRMSystem.Infrastructure.Data.Seeding;

namespace CRMSystem.API.IntegrationTests.Infrastructure.Seeding;

public static class BaselineSeed
{
    public static async Task SeedAsync(IServiceProvider sp)
    {
        await RoleSeeder.SeedRolesAsync(sp);
        await TicketCategorySeeder.SeedTicketCategoriesAsync(sp);
        await PrioritySeeder.SeedPrioritiesAsync(sp);
        await CommunicationChannelSeeder.SeedChannelsAsync(sp);
    }
}