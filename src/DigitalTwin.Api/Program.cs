using DigitalTwin.Api.HostedServices;
using DigitalTwin.Infrastructure;
using DigitalTwin.Infrastructure.Sync;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<PrinterCatalogSyncService>();
builder.Services.AddScoped<PrinterActivitySyncService>();

builder.Services.AddHostedService<PrinterCatalogSyncWorker>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();