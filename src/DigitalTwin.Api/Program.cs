using DigitalTwin.Api.HostedServices;
using DigitalTwin.Infrastructure;
using DigitalTwin.Infrastructure.Sync;
using DigitalTwin.Infrastructure.Queries;
using DigitalTwin.Infrastructure.Simulation;
using DigitalTwin.Api.HostedServices;
using DigitalTwin.Api.Streaming;
using DigitalTwin.Application.Abstractions.Telemetry;
using DigitalTwin.Infrastructure.Telemetry;
using DigitalTwin.Infrastructure.Scheduling;
using DigitalTwin.Application.Abstractions.Inventory;
using DigitalTwin.Infrastructure.Inventory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<PrinterCatalogSyncService>();
builder.Services.AddScoped<PrinterActivitySyncService>();
builder.Services.AddScoped<PrinterReadService>();
builder.Services.AddScoped<PrinterSimulationService>();
builder.Services.AddScoped<TaskTelemetrySummaryService>();
builder.Services.AddScoped<ScheduledPrintJobReadService>();
builder.Services.AddScoped<PrintSchedulerService>();

builder.Services.AddSingleton<IPrinterTelemetryPublisher, InMemoryTelemetryPublisher>();
builder.Services.AddSingleton<IPrinterTelemetryWriter, InfluxPrinterTelemetryWriter>();
builder.Services.AddSingleton<PrinterTelemetryGenerator>();

builder.Services.AddHostedService<PrinterCatalogSyncWorker>();
builder.Services.AddHostedService<PrinterSimulationCompletionWorker>();
builder.Services.AddHostedService<PrinterSimulationTelemetryWorker>();
// if (builder.Configuration.GetValue<bool>("Workers:EnablePrintSchedulingWorker"))
// {
//     builder.Services.AddHostedService<PrintSchedulingWorker>();
// }
builder.Services.AddSingleton<IZoneInventoryPublisher, InMemoryZoneInventoryPublisher>();
builder.Services.AddScoped<CvZoneStateService>();

builder.Services.AddHostedService<CvEventsConsumerWorker>();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();