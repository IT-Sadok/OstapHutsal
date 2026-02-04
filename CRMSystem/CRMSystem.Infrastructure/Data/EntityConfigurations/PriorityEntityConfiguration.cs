using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Infrastructure.Data.EntityConfigurations;

public class PriorityEntityConfiguration : IEntityTypeConfiguration<Priority>
{
    public void Configure(EntityTypeBuilder<Priority> builder)
    {
        builder.ToTable("priorities");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.HasIndex(p => p.Type)
            .IsUnique();
    }
}