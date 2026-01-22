using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Infrastructure.Data.EntityConfigurations;

public class AgentEntityConfiguration: IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        builder.ToTable("agents");
        
        builder.HasKey(u => u.Id);
        
        builder.HasOne(u => u.Actor)
            .WithOne(a => a.Agent)
            .HasForeignKey<Agent>(u => u.ActorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}