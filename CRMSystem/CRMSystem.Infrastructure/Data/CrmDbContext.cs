using CRMSystem.Domain.Entities;
using CRMSystem.Domain.Entities.Base;
using CRMSystem.Infrastructure.Data.EntityConfigurations;
using CRMSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Infrastructure.Data;

public sealed class CrmDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public CrmDbContext(DbContextOptions<CrmDbContext> options):base(options)
    {
    }
    
    public DbSet<Actor> Actors => Set<Actor>();
    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<Client>  Clients => Set<Client>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ActorEntityConfiguration());
        builder.ApplyConfiguration(new ClientEntityConfiguration());
        builder.ApplyConfiguration(new AgentEntityConfiguration());
        builder.ApplyConfiguration(new ApplicationUserConfiguration());
    }
}