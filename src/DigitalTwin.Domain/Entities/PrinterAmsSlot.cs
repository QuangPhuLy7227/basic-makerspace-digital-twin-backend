namespace DigitalTwin.Domain.Entities;

public class PrinterAmsSlot
{
    public Guid Id { get; set; }

    public Guid PrinterAmsUnitId { get; set; }
    public PrinterAmsUnit PrinterAmsUnit { get; set; } = null!;

    public int SlotIndex { get; set; }                  // tray id
    public string? TrayType { get; set; }               // PLA, PETG, etc.
    public string? TrayColor { get; set; }              // RGBA hex
    public int? RemainingLengthMm { get; set; }         // remain
    public int? TotalLengthMm { get; set; }             // total_len
    public int? NozzleTempMinC { get; set; }
    public int? NozzleTempMaxC { get; set; }

    public DateTimeOffset LastSeenAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
}