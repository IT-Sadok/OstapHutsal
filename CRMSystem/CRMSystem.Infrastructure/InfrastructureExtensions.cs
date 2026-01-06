using CRMSystem.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;


namespace CRMSystem.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<CrmDbContext>(options =>
        {
            
        });
        
        return services;
    }
}