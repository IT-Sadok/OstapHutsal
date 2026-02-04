using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Infrastructure.Data.EntityConfigurations;

public class TicketCategoryEntityConfiguration : IEntityTypeConfiguration<TicketCategory>
{
    public void Configure(EntityTypeBuilder<TicketCategory> builder)
    {
        builder.ToTable("ticket_categories");

        builder.HasKey(tk => tk.Id);

        builder.Property(tk => tk.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(tc => tc.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(tc => tc.Description)
            .HasMaxLength(512);

        builder.HasIndex(tc => tc.Type);
    }
}