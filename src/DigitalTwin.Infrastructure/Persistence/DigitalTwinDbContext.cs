using DigitalTwin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalTwin.Infrastructure.Persistence;

public class DigitalTwinDbContext : DbContext
{
    public DigitalTwinDbContext(DbContextOptions<DigitalTwinDbContext> options)
        : base(options)
    {
    }

    public DbSet<Printer> Printers => Set<Printer>();
    public DbSet<PrinterFirmwareStatus> PrinterFirmwareStatuses => Set<PrinterFirmwareStatus>();
    public DbSet<PrinterAmsUnit> PrinterAmsUnits => Set<PrinterAmsUnit>();

    public DbSet<PrinterTask> PrinterTasks => Set<PrinterTask>();
    public DbSet<PrinterTaskAmsDetail> PrinterTaskAmsDetails => Set<PrinterTaskAmsDetail>();
    public DbSet<PrinterMessage> PrinterMessages => Set<PrinterMessage>();

    public DbSet<PrinterSimulationControl> PrinterSimulationControls => Set<PrinterSimulationControl>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DigitalTwinDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}