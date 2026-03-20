using DigitalTwin.Infrastructure.Simulation;
using Microsoft.AspNetCore.Mvc;

namespace DigitalTwin.Api.Controllers;

[ApiController]
[Route("api/printers/{deviceId}/simulate")]
public class PrinterSimulationController : ControllerBase
{
    [HttpPost("start")]
    public async Task<IActionResult> StartPrinter(
        string deviceId,
        [FromBody] StartPrinterSimulationRequest request,
        [FromServices] PrinterSimulationService simulationService,
        CancellationToken cancellationToken)
    {
        var result = await simulationService.StartPrinterAsync(
            deviceId,
            request.DesignTitle,
            request.SimulatedDurationSeconds,
            cancellationToken);

        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }

    [HttpPost("stop")]
    public async Task<IActionResult> StopPrinter(
        string deviceId,
        [FromServices] PrinterSimulationService simulationService,
        CancellationToken cancellationToken)
    {
        var result = await simulationService.StopPrinterAsync(deviceId, cancellationToken);

        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }
}

public class StartPrinterSimulationRequest
{
    public string? DesignTitle { get; set; }
    public int SimulatedDurationSeconds { get; set; } = 300;
}