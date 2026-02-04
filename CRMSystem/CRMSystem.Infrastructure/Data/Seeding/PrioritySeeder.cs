using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CRMSystem.Infrastructure.Data.Seeding;

public static class PrioritySeeder
{
    public static async Task SeedPrioritiesAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<CrmDbContext>();
        var priorityRepository = serviceProvider.GetRequiredService<IPriorityRepository>();

        if (await context.Priorities.AnyAsync())
        {
            return;
        }

        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var priorities = Enum.GetValues<PriorityType>()
                .Select(p => new Priority
                {
                    Type = p
                });

            await priorityRepository.AddRangeAsync(priorities);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}