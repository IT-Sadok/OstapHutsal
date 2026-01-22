using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Infrastructure.Data.EntityConfigurations;

public class ActorEntityConfiguration: IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.ToTable("actors");
        
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Kind)
            .IsRequired()
            .HasConversion<int>();
        
        builder.HasIndex(a => a.Kind);
    }
}