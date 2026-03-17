using DigitalTwin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalTwin.Infrastructure.Persistence.Configurations;

public class PrinterAmsSlotConfiguration : IEntityTypeConfiguration<PrinterAmsSlot>
{
    public void Configure(EntityTypeBuilder<PrinterAmsSlot> builder)
    {
        builder.ToTable("printer_ams_slots");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.PrinterAmsUnitId, x.SlotIndex }).IsUnique();

        builder.Property(x => x.TrayType).HasMaxLength(64);
        builder.Property(x => x.TrayColor).HasMaxLength(32);
    }
}