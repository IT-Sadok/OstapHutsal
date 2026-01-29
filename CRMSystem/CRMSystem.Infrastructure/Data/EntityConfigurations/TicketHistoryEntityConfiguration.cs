using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Infrastructure.Data.EntityConfigurations;

public class TicketHistoryEntityConfiguration : IEntityTypeConfiguration<TicketHistory>
{
    public void Configure(EntityTypeBuilder<TicketHistory> builder)
    {
        builder.ToTable("ticket_history");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.EventType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(h => h.FieldName)
            .HasMaxLength(128);
        builder.Property(h => h.OldValue)
            .HasMaxLength(512);
        builder.Property(h => h.NewValue)
            .HasMaxLength(512);

        builder.HasIndex(th => th.TicketId);

        builder.Property(h => h.Metadata)
            .HasColumnType("jsonb");

        builder.HasOne(h => h.Ticket)
            .WithMany(t => t.History)
            .HasForeignKey(h => h.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(h => h.Actor)
            .WithMany(t => t.History)
            .HasForeignKey(h => h.ActorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}