using System.Text.Json.Serialization;

namespace DigitalTwin.Application.Printers.Dtos;

public class BindResponseDto
{
    [JsonPropertyName("devices")]
    public List<BindPrinterDto> Devices { get; set; } = new();
}

public class BindPrinterDto
{
    [JsonPropertyName("dev_id")]
    public string DeviceId { get; set; } = null!;

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("online")]
    public bool IsOnline { get; set; }

    [JsonPropertyName("print_status")]
    public string? PrintStatus { get; set; }

    [JsonPropertyName("dev_model_name")]
    public string? ModelName { get; set; }

    [JsonPropertyName("dev_product_name")]
    public string? ProductName { get; set; }

    [JsonPropertyName("dev_structure")]
    public string? Structure { get; set; }

    [JsonPropertyName("nozzle_diameter")]
    public decimal? NozzleDiameter { get; set; }

    [JsonPropertyName("ams")]
    public List<BindAmsDto>? Ams { get; set; }
}

public class BindAmsDto
{
    [JsonPropertyName("humidity")]
    public string? Humidity { get; set; }

    [JsonPropertyName("temp")]
    public string? Temperature { get; set; }

    [JsonPropertyName("tray")]
    public List<BindAmsTrayDto>? Trays { get; set; }
}

public class BindAmsTrayDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("tray_type")]
    public string? TrayType { get; set; }

    [JsonPropertyName("tray_color")]
    public string? TrayColor { get; set; }

    [JsonPropertyName("remain")]
    public int? Remain { get; set; }

    [JsonPropertyName("total_len")]
    public int? TotalLength { get; set; }

    [JsonPropertyName("nozzle_temp_min")]
    public int? NozzleTempMin { get; set; }

    [JsonPropertyName("nozzle_temp_max")]
    public int? NozzleTempMax { get; set; }
}