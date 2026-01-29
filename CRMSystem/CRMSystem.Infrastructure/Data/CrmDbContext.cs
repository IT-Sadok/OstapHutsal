using CRMSystem.Application.Identity;
using CRMSystem.Domain.Entities;
using CRMSystem.Domain.Entities.Base;
using CRMSystem.Infrastructure.Data.EntityConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Infrastructure.Data;

public sealed class CrmDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
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
    public DbSet<TicketHistory> TicketHistory => Set<TicketHistory>();
    public DbSet<AgentNotification> Notifications => Set<AgentNotification>();

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
}