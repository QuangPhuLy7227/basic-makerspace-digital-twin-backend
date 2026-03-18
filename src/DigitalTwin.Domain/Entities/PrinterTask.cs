namespace DigitalTwin.Domain.Entities;

public class PrinterTask
{
    public Guid Id { get; set; }

    public long ExternalTaskId { get; set; }              // tasks[].id
    public Guid? PrinterId { get; set; }
    public Printer? Printer { get; set; }

    public string DeviceId { get; set; } = null!;
    public string? DeviceName { get; set; }
    public string? DeviceModel { get; set; }

    public string? DesignTitle { get; set; }
    public string? DesignTitleTranslated { get; set; }
    public string? StatusText { get; set; }               // optional normalized later
    public int? FailedType { get; set; }
    public string? Mode { get; set; }
    public string? BedType { get; set; }

    public int? CostTimeSeconds { get; set; }
    public int? LengthMm { get; set; }
    public decimal? WeightGrams { get; set; }

    public DateTimeOffset? StartTimeUtc { get; set; }
    public DateTimeOffset? EndTimeUtc { get; set; }

    public string? CoverUrl { get; set; }

    public string RawJson { get; set; } = null!;

    public DateTimeOffset SourceUpdatedAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }

    public ICollection<PrinterTaskAmsDetail> AmsDetails { get; set; } = new List<PrinterTaskAmsDetail>();
}