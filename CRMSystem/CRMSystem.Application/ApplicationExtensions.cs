using CRMSystem.Application.Abstractions.DomainEvents;
using CRMSystem.Application.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace CRMSystem.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(ApplicationExtensions))
            .AddClasses(
                filter => filter.Where(x => x.Name.EndsWith("Service")),
                publicOnly: false)
            .UsingRegistrationStrategy(RegistrationStrategy.Throw)
            .AsMatchingInterface()
            .WithScopedLifetime());

        services.Scan(scan => scan.FromAssembliesOf(typeof(ApplicationExtensions))
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}