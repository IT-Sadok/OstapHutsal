using CRMSystem.API.Endpoints.Auth;
using CRMSystem.Infrastructure.Data;
using CRMSystem.Infrastructure.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace CRMSystem.API.Common.Extensions;

public static class WebApplicationExtensions
{
    public static async Task ApplyMigrationsAndSeedAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CrmDbContext>();
        await dbContext.Database.MigrateAsync();

        await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
        await TicketCategorySeeder.SeedTicketCategoriesAsync(scope.ServiceProvider);
    }

    public static void MapEndpoints(this WebApplication app)
    {
        app.MapAuthEndpoints();
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT token in the format: Bearer {token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}