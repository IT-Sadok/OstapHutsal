using CRMSystem.Application.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Infrastructure.Data.EntityConfigurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.ActorId)
            .IsRequired();

        builder.HasIndex(u => u.ActorId)
            .IsUnique();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasOne(u => u.Actor)
            .WithOne()
            .HasForeignKey<ApplicationUser>(u => u.ActorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}