using CRMSystem.Application.Abstractions.Services;
using CRMSystem.Application.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace CRMSystem.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}