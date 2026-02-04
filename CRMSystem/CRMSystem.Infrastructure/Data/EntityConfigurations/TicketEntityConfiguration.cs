using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace CRMSystem.Infrastructure.Data.EntityConfigurations;

public class TicketEntityConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");

        builder.HasKey(p => p.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(t => t.Description)
            .HasColumnType("text");

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(t => t.SourceDetails)
            .HasColumnType("jsonb");

        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.AssignedToActorId);
        builder.HasIndex(t => t.ClientId);
        builder.HasIndex(t => t.CategoryId);
        builder.HasIndex(t => t.PriorityId);
        builder.HasIndex(t => t.ChannelId);
        builder.HasIndex(t => t.CreatedAt);

        builder.HasOne(t => t.Client)
            .WithMany(c => c.Tickets)
            .HasForeignKey(t => t.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Category)
            .WithMany(c => c.Tickets)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Priority)
            .WithMany(p => p.Tickets)
            .HasForeignKey(t => t.PriorityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.CommunicationChannel)
            .WithMany(c => c.Tickets)
            .HasForeignKey(t => t.ChannelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.AssignedToActor)
            .WithMany(a => a.Tickets)
            .HasForeignKey(t => t.AssignedToActorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.Order)
            .WithMany(o => o.Tickets)
            .HasForeignKey(t => t.OrderId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}