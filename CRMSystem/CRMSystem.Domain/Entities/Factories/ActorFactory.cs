using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities.Factories;

public static class ActorFactory
{
    public static Actor CreateAgentActor()
    {
        var actor = new Actor
        {
            Kind = ActorKind.Agent
        };

        var agent = new Agent
        {
            IsActive = true,
            Actor = actor
        };

        actor.Agent = agent;

        return actor;
    }

    public static Actor CreateClientActor(string? phone = null, string? address = null)
    {
        var actor = new Actor
        {
            Kind = ActorKind.Client
        };

        var client = new Client
        {
            Phone = phone,
            Address = address,
            Actor = actor
        };

        actor.Client = client;

        return actor;
    }
}