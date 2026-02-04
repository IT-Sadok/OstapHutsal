using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Infrastructure.Data.EntityConfigurations;

public class TicketSnapshotEntityConfiguration : IEntityTypeConfiguration<TicketSnapshot>
{
    public void Configure(EntityTypeBuilder<TicketSnapshot> builder)
    {
        builder.ToTable("ticket_snapshots");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PayloadJson)
            .HasColumnType("jsonb");

        builder.HasIndex(x => new { x.TicketId, x.Version })
            .IsUnique();

        builder.HasOne(x => x.Ticket)
            .WithMany(t => t.Snapshots)
            .HasForeignKey(x => x.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ChangedByActor)
            .WithMany(t => t.Snapshots)
            .HasForeignKey(x => x.ChangedByActorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}