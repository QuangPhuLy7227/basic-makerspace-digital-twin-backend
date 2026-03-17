using DigitalTwin.Infrastructure.Sync;
using Microsoft.AspNetCore.Mvc;

namespace DigitalTwin.Api.Controllers;

[ApiController]
[Route("api/admin/sync")]
public class AdminSyncController : ControllerBase
{
    [HttpPost("bind")]
    public async Task<IActionResult> SyncBind(
        [FromServices] PrinterCatalogSyncService syncService,
        CancellationToken cancellationToken)
    {
        await syncService.SyncBindAsync(cancellationToken);
        return Ok(new { message = "Bind sync completed." });
    }

    [HttpPost("version")]
    public async Task<IActionResult> SyncVersion(
        [FromServices] PrinterCatalogSyncService syncService,
        CancellationToken cancellationToken)
    {
        await syncService.SyncVersionAsync(cancellationToken);
        return Ok(new { message = "Version sync completed." });
    }

    [HttpPost("all")]
    public async Task<IActionResult> SyncAll(
        [FromServices] PrinterCatalogSyncService syncService,
        CancellationToken cancellationToken)
    {
        await syncService.SyncBindAsync(cancellationToken);
        await syncService.SyncVersionAsync(cancellationToken);
        return Ok(new { message = "Full sync completed." });
    }
}