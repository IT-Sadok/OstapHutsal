using CRMSystem.API.Endpoints.Auth;
using CRMSystem.API.Endpoints.Tickets;
using CRMSystem.API.SignalR;
using CRMSystem.API.SignalR.Hubs;
using CRMSystem.Application;
using CRMSystem.Application.Abstractions.SignalR;
using CRMSystem.Application.Features.Auth;
using CRMSystem.Application.SignalR.Protocol;
using CRMSystem.Infrastructure.Data;
using CRMSystem.Infrastructure.Data.Seeding;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;
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
        await PrioritySeeder.SeedPrioritiesAsync(scope.ServiceProvider);
        await CommunicationChannelSeeder.SeedChannelsAsync(scope.ServiceProvider);
    }

    public static void MapEndpoints(this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapTicketsEndpoints();
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

    public static void ConfigureSignalR(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddSingleton<IRealtimeNotifier, RealtimeNotifier>();
        services.AddSingleton<IUserIdProvider, ActorIdUserIdProvider>();
    }

    public static void MapSignalRHubs(this WebApplication app)
    {
        app.MapHub<NotificationsHub>(HubRoutes.Notifications);
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ApplicationExtensions).Assembly);
    }
}