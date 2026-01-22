using CRMSystem.Application.Abstractions.Persistence;
using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Application.Common.Authorization;
using CRMSystem.Application.Common.Exceptions;
using CRMSystem.Domain.Entities.Factories;
using CRMSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CRMSystem.Infrastructure.Identity;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var actorRepository = serviceProvider.GetRequiredService<IActorRepository>();
        var context = serviceProvider.GetRequiredService<CrmDbContext>();

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var roles = new[] { Roles.SuperAdmin, Roles.Admin, Roles.Operator, Roles.Client };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new ApplicationRole(role));
            }

            var adminEmail = "superadmin@gmail.com";
            var adminPassword = "Admin@123";

            var adminExists = await userManager.FindByEmailAsync(adminEmail);
            if (adminExists == null)
            {
                var actor = ActorFactory.CreateAgentActor();
                await actorRepository.AddAsync(actor);
                await context.SaveChangesAsync();

                var adminUser = new ApplicationUser
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    UserName = "superAdmin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Actor = actor
                };

                var identityResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (!identityResult.Succeeded)
                    throw new IdentitySeedingException("Failed to create the super admin user: " +
                                                       string.Join(", ", identityResult.Errors));

                var addToToleResult = await userManager.AddToRoleAsync(adminUser, Roles.SuperAdmin);
                if (!addToToleResult.Succeeded)
                    throw new IdentitySeedingException("Failed to create the super admin user: " +
                                                       string.Join(", ", addToToleResult.Errors));
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}