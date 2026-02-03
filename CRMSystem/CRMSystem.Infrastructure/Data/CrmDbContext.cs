using CRMSystem.Application.Abstractions.DomainEvents;
using CRMSystem.Application.Identity;
using CRMSystem.Domain.DomainEvents;
using CRMSystem.Domain.Entities;
using CRMSystem.Domain.Entities.Base;
using CRMSystem.Infrastructure.Data.EntityConfigurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Infrastructure.Data;

public sealed class CrmDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public CrmDbContext(DbContextOptions<CrmDbContext> options) :
        base(options)
    {
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
    public DbSet<TicketSnapshot> TicketSnapshots => Set<TicketSnapshot>();
    public DbSet<ActorNotification> Notifications => Set<ActorNotification>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(CrmDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}