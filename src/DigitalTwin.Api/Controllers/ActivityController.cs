using DigitalTwin.Infrastructure.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DigitalTwin.Api.Controllers;

[ApiController]
[Route("api")]
public class ActivityController : ControllerBase
{
    [HttpGet("tasks")]
    public async Task<IActionResult> GetAllTasks(
        [FromServices] PrinterReadService readService,
        CancellationToken cancellationToken)
    {
        return Ok(await readService.GetAllTasksAsync(cancellationToken));
    }

    [HttpGet("tasks/{externalTaskId:long}")]
    public async Task<IActionResult> GetTaskByExternalTaskId(
        long externalTaskId,
        [FromServices] PrinterReadService readService,
        CancellationToken cancellationToken)
    {
        var result = await readService.GetTaskByExternalTaskIdAsync(externalTaskId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("messages")]
    public async Task<IActionResult> GetAllMessages(
        [FromServices] PrinterReadService readService,
        CancellationToken cancellationToken)
    {
        return Ok(await readService.GetAllMessagesAsync(cancellationToken));
    }

    [HttpGet("messages/{externalMessageId:long}")]
    public async Task<IActionResult> GetMessageByExternalMessageId(
        long externalMessageId,
        [FromServices] PrinterReadService readService,
        CancellationToken cancellationToken)
    {
        var result = await readService.GetMessageByExternalMessageIdAsync(externalMessageId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("task-ams-details")]
    public async Task<IActionResult> GetAllTaskAmsDetails(
        [FromServices] PrinterReadService readService,
        CancellationToken cancellationToken)
    {
        return Ok(await readService.GetAllTaskAmsDetailsAsync(cancellationToken));
    }

    [HttpGet("task-ams-details/by-task/{externalTaskId:long}")]
    public async Task<IActionResult> GetTaskAmsDetailsByExternalTaskId(
        long externalTaskId,
        [FromServices] PrinterReadService readService,
        CancellationToken cancellationToken)
    {
        return Ok(await readService.GetTaskAmsDetailsByExternalTaskIdAsync(externalTaskId, cancellationToken));
    }

    [HttpGet("tasks/{externalTaskId:long}/summary")]
    public async Task<IActionResult> GetTaskSummary(
        long externalTaskId,
        [FromServices] TaskTelemetrySummaryService summaryService,
        CancellationToken cancellationToken)
    {
        var result = await summaryService.GetTaskSummaryAsync(externalTaskId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}