using DigitalTwin.Infrastructure.Queries;
using DigitalTwin.Infrastructure.Inventory;
using DigitalTwin.Application.Inventory.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DigitalTwin.Api.Controllers;

[ApiController]
[Route("api/printers")]
public class PrintersController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPrinters(
        [FromServices] PrinterReadService readService,
        CancellationToken cancellationToken)
    {
        return Ok(await readService.GetPrintersAsync(cancellationToken));
    }

    [HttpGet("running")]
    public async Task<IActionResult> GetRunningPrinters(
        [FromServices] PrinterReadService readService,
        CancellationToken cancellationToken)
    {
        return Ok(await readService.GetRunningPrintersAsync(cancellationToken));
    }

    [HttpGet("{deviceId}")]
    public async Task<IActionResult> GetPrinterDetail(
        string deviceId,
        [FromServices] PrinterReadService readService,
        CancellationToken cancellationToken)
    {
        var result = await readService.GetPrinterDetailAsync(deviceId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{deviceId}/firmware")]
    public async Task<IActionResult> GetPrinterFirmware(
        string deviceId,
        [FromServices] PrinterReadService readService,
        CancellationToken cancellationToken)
    {
        var result = await readService.GetPrinterFirmwareAsync(deviceId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{deviceId}/ams-units")]
    public async Task<IActionResult> GetPrinterAmsUnits(
        string deviceId,
        [FromServices] PrinterReadService readService,
        CancellationToken cancellationToken)
    {
        return Ok(await readService.GetPrinterAmsUnitsAsync(deviceId, cancellationToken));
    }

    [HttpGet("{deviceId}/tasks")]
    public async Task<IActionResult> GetPrinterTasks(
        string deviceId,
        [FromServices] PrinterReadService readService,
        CancellationToken cancellationToken)
    {
        return Ok(await readService.GetPrinterTasksAsync(deviceId, cancellationToken));
    }

    [HttpGet("{deviceId}/messages")]
    public async Task<IActionResult> GetPrinterMessages(
        string deviceId,
        [FromServices] PrinterReadService readService,
        CancellationToken cancellationToken)
    {
        return Ok(await readService.GetPrinterMessagesAsync(deviceId, cancellationToken));
    }

    [HttpGet("{deviceId}/timeline")]
    public async Task<IActionResult> GetPrinterTimeline(
        string deviceId,
        [FromServices] PrinterReadService readService,
        CancellationToken cancellationToken)
    {
        return Ok(await readService.GetPrinterTimelineAsync(deviceId, cancellationToken));
    }

    [HttpGet("{deviceId}/loaded-spools")]
    public async Task<IActionResult> GetLoadedSpools(
        string deviceId,
        [FromServices] CvZoneStateService service,
        CancellationToken cancellationToken)
    {
        var result = await service.GetPrinterLoadedSpoolsAsync(deviceId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{deviceId}/replacement-suggestions")]
    public async Task<IActionResult> GetReplacementSuggestions(
        string deviceId,
        [FromQuery] decimal lowThresholdPercent,
        [FromServices] CvZoneStateService service,
        CancellationToken cancellationToken)
    {
        var threshold = lowThresholdPercent <= 0 ? 15m : lowThresholdPercent;

        var result = await service.GetReplacementSuggestionsAsync(deviceId, threshold, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("{deviceId}/loaded-spools")]
    public async Task<IActionResult> UpdateLoadedSpools(
        string deviceId,
        [FromBody] UpdatePrinterLoadedSpoolsRequest request,
        [FromServices] CvZoneStateService service,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await service.UpdatePrinterLoadedSpoolsAsync(deviceId, request, cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}