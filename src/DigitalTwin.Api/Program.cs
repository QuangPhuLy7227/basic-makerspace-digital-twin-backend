using DigitalTwin.Api.HostedServices;
using DigitalTwin.Infrastructure;
using DigitalTwin.Infrastructure.Sync;
using DigitalTwin.Infrastructure.Queries;
using DigitalTwin.Infrastructure.Simulation;
using DigitalTwin.Api.HostedServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<PrinterCatalogSyncService>();
builder.Services.AddScoped<PrinterActivitySyncService>();
builder.Services.AddScoped<PrinterReadService>();
builder.Services.AddScoped<PrinterSimulationService>();

builder.Services.AddHostedService<PrinterCatalogSyncWorker>();
builder.Services.AddHostedService<PrinterSimulationCompletionWorker>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();