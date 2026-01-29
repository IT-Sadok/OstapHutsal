using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Infrastructure.Data.EntityConfigurations;

public class AgentNotificationEntityConfiguration : IEntityTypeConfiguration<AgentNotification>
{
    public void Configure(EntityTypeBuilder<AgentNotification> builder)
    {
        builder.ToTable("agent_notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(n => n.Payload)
            .HasColumnType("jsonb");

        builder.Property(n => n.ReadState)
            .IsRequired()
            .HasConversion<int>();

        builder.HasOne(n => n.Agent)
            .WithMany(a => a.Notifications)
            .HasForeignKey(n => n.AgentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(n => n.Ticket)
            .WithMany(a => a.Notifications)
            .HasForeignKey(n => n.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}