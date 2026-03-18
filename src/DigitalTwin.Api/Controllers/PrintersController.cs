using DigitalTwin.Infrastructure.Queries;
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
}