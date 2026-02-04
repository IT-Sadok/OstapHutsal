using System.Text;
using CRMSystem.Application.Abstractions.DomainEvents;
using CRMSystem.Application.Abstractions.Identity;
using CRMSystem.Application.Abstractions.Persistence;
using CRMSystem.Application.Abstractions.Security;
using CRMSystem.Application.Abstractions.Services;
using CRMSystem.Application.Common.Authorization;
using CRMSystem.Application.Common.Security;
using CRMSystem.Application.Identity;
using CRMSystem.Application.SignalR.Protocol;
using Microsoft.AspNetCore.Identity;
using CRMSystem.Infrastructure.Data;
using CRMSystem.Infrastructure.DomainEvents;
using CRMSystem.Infrastructure.Options;
using CRMSystem.Infrastructure.Security.Jwt;
using CRMSystem.Infrastructure.Security.UserContextProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Scrutor;

namespace CRMSystem.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddIdentityConfiguration();
        services.AddJwtAuthentication(configuration);
        services.AddAuthorizationPolicies();
        services.AddInfrastructureRegistrations();
        services.ConfigureOptions(configuration);
    }

    private static void AddDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgresDb");

        services.AddDbContext<CrmDbContext>(options =>
            options.UseNpgsql(connectionString));
    }

    private static void AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<CrmDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        });
    }

    private static void AddJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration
            .GetSection(nameof(JwtOptions))
            .Get<JwtOptions>()!;

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query[SignalRQueryParameters.AccessToken];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments(HubRoutes.Notifications))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
    }

    private static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.Admin, policy =>
                policy.RequireRole(Roles.Admin, Roles.SuperAdmin));

            options.AddPolicy(Policies.SuperAdmin, policy =>
                policy.RequireRole(Roles.SuperAdmin));

            options.AddPolicy(Policies.Client, policy =>
                policy.RequireRole(Roles.Client));

            options.AddPolicy(Policies.Operator, policy =>
                policy.RequireRole(Roles.Operator));

            options.AddPolicy(Policies.OperatorOrAdmin, p =>
                p.RequireRole(Roles.Operator, Roles.Admin, Roles.SuperAdmin));
        });
    }

    private static void AddInfrastructureRegistrations(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Scan(scan => scan
            .FromAssemblies(typeof(InfrastructureExtensions).Assembly)
            .AddClasses(
                filter => filter.Where(x => x.Name.EndsWith("Repository")),
                publicOnly: false)
            .UsingRegistrationStrategy(RegistrationStrategy.Throw)
            .AsMatchingInterface()
            .WithScopedLifetime());

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUserContextProvider, UserContextProvider>();

        services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();
    }

    private static void ConfigureOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(
            configuration.GetSection(nameof(JwtOptions)));
    }
}