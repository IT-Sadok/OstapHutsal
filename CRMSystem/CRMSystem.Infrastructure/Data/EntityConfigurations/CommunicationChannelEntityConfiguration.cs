using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Infrastructure.Data.EntityConfigurations;

public class CommunicationChannelEntityConfiguration : IEntityTypeConfiguration<CommunicationChannel>
{
    public void Configure(EntityTypeBuilder<CommunicationChannel> builder)
    {
        builder.ToTable("communication_channels");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.ChannelType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(c => c.Description)
            .HasMaxLength(512);

        builder.HasIndex(c => c.ChannelType)
            .IsUnique();
    }
}