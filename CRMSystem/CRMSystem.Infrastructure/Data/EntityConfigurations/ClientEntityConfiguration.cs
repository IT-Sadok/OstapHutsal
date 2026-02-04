using CRMSystem.Domain.Entities;
using CRMSystem.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Infrastructure.Data.EntityConfigurations;

public class ClientEntityConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Phone)
            .HasMaxLength(32);

        builder.Property(c => c.Address)
            .HasMaxLength(512);

        builder.Property(c => c.ExternalId)
            .HasMaxLength(128);

        builder.Property(c => c.ActorId)
            .IsRequired();

        builder.HasIndex(c => c.ActorId)
            .IsUnique();

        builder.HasIndex(c => c.Phone).IsUnique();

        builder.HasOne(c => c.Actor)
            .WithOne(a => a.Client)
            .HasForeignKey<Client>(c => c.ActorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}