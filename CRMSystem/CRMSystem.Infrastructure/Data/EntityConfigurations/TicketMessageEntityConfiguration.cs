using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Infrastructure.Data.EntityConfigurations;

public class TicketMessageEntityConfiguration : IEntityTypeConfiguration<TicketMessage>
{
    public void Configure(EntityTypeBuilder<TicketMessage> builder)
    {
        builder.ToTable("ticket_messages");

        builder.HasKey(tm => tm.Id);

        builder.Property(tm => tm.Text)
            .HasColumnType("text");

        builder.Property(tm => tm.MessageType)
            .IsRequired()
            .HasConversion<int>();

        builder.HasIndex(tm => tm.MessageType);
        builder.HasIndex(tm => tm.TicketId);
        builder.HasIndex(tm => tm.SenderActorId);

        builder.HasOne(tm => tm.Ticket)
            .WithMany(t => t.Messages)
            .HasForeignKey(tm => tm.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tm => tm.CommunicationChannel)
            .WithMany(c => c.Messages)
            .HasForeignKey(t => t.ChannelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(tm => tm.SenderActor)
            .WithMany(a => a.Messages)
            .HasForeignKey(t => t.SenderActorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}