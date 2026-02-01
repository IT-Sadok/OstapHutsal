using CRMSystem.Application.Abstractions.DomainEvents;
using CRMSystem.Application.Identity;
using CRMSystem.Domain.DomainEvents;
using CRMSystem.Domain.Entities;
using CRMSystem.Domain.Entities.Base;
using CRMSystem.Infrastructure.Data.EntityConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Infrastructure.Data;

public sealed class CrmDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private readonly IDomainEventsDispatcher _domainEventsDispatcher;

    public CrmDbContext(DbContextOptions<CrmDbContext> options, IDomainEventsDispatcher domainEventsDispatcher) :
        base(options)
    {
        _domainEventsDispatcher = domainEventsDispatcher;
    }

    public DbSet<Actor> Actors => Set<Actor>();
    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TicketCategory> TicketCategories => Set<TicketCategory>();
    public DbSet<Priority> Priorities => Set<Priority>();
    public DbSet<CommunicationChannel> Channels => Set<CommunicationChannel>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<TicketMessage> TicketMessages => Set<TicketMessage>();
    public DbSet<TicketHistory> TicketHistory => Set<TicketHistory>();
    public DbSet<ActorNotification> Notifications => Set<ActorNotification>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ActorEntityConfiguration());
        builder.ApplyConfiguration(new ClientEntityConfiguration());
        builder.ApplyConfiguration(new AgentEntityConfiguration());
        builder.ApplyConfiguration(new ApplicationUserConfiguration());
        builder.ApplyConfiguration(new TicketEntityConfiguration());
        builder.ApplyConfiguration(new TicketCategoryEntityConfiguration());
        builder.ApplyConfiguration(new PriorityEntityConfiguration());
        builder.ApplyConfiguration(new OrderEntityConfiguration());
        builder.ApplyConfiguration(new CommunicationChannelEntityConfiguration());
        builder.ApplyConfiguration(new AgentNotificationEntityConfiguration());
        builder.ApplyConfiguration(new TicketHistoryEntityConfiguration());
        builder.ApplyConfiguration(new TicketMessageEntityConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEvents();

        return result;
    }

    private async Task PublishDomainEvents()
    {
        var domainEvents = ChangeTracker
            .Entries<BaseEntity<Guid>>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                List<IDomainEvent> domainEvents = entity.DomainEvents.ToList();
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .ToList();

        await _domainEventsDispatcher.DispatchAsync(domainEvents);
    }
}