using DigitalTwin.Infrastructure.Sync;

namespace DigitalTwin.Api.HostedServices;

public class PrinterCatalogSyncWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PrinterCatalogSyncWorker> _logger;

    public PrinterCatalogSyncWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<PrinterCatalogSyncWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PrinterCatalogSyncWorker started.");

        // Run immediately once at startup
        await RunSyncCycleAsync(stoppingToken);

        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(5));

        while (!stoppingToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(stoppingToken))
        {
            await RunSyncCycleAsync(stoppingToken);
        }

        _logger.LogInformation("PrinterCatalogSyncWorker stopped.");
    }

    private async Task RunSyncCycleAsync(CancellationToken cancellationToken)
    {
        try
        {
            using (var bindScope = _scopeFactory.CreateScope())
            {
                var bindSyncService = bindScope.ServiceProvider.GetRequiredService<PrinterCatalogSyncService>();
                await bindSyncService.SyncBindAsync(cancellationToken);
            }

            using (var versionScope = _scopeFactory.CreateScope())
            {
                var versionSyncService = versionScope.ServiceProvider.GetRequiredService<PrinterCatalogSyncService>();
                await versionSyncService.SyncVersionAsync(cancellationToken);
            }

            _logger.LogInformation("Printer catalog sync completed successfully at {Time}.", DateTimeOffset.UtcNow);
        }
        catch (OperationCanceledException)
        {
            // normal shutdown
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Printer catalog sync failed.");
        }
    }
}