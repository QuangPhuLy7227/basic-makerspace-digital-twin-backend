using System.Text.Json;
using DigitalTwin.Application.Abstractions.Inventory;
using DigitalTwin.Application.Inventory.Dtos;
using DigitalTwin.Domain.Entities;
using DigitalTwin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DigitalTwin.Infrastructure.Inventory;

public class CvZoneStateService
{
    private readonly DigitalTwinDbContext _db;
    private readonly IZoneInventoryPublisher _publisher;

    public CvZoneStateService(
        DigitalTwinDbContext db,
        IZoneInventoryPublisher publisher)
    {
        _db = db;
        _publisher = publisher;
    }

    public async Task UpsertInventoryMessageAsync(
        CvZoneInventoryMessageDto message,
        CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;

        foreach (var zoneEntry in message.Zones)
        {
            var zoneName = zoneEntry.Key;
            var zone = zoneEntry.Value;

            var state = await _db.CvZoneStates.FirstOrDefaultAsync(
                x => x.CameraId == message.CameraId && x.ZoneName == zoneName,
                cancellationToken);

            if (state is null)
            {
                state = new CvZoneState
                {
                    Id = Guid.NewGuid(),
                    CameraId = message.CameraId,
                    ZoneName = zoneName,
                    CreatedAtUtc = now
                };

                _db.CvZoneStates.Add(state);
            }

            state.SpoolIdsJson = JsonSerializer.Serialize(zone.SpoolIds);
            state.UnknownSpoolCount = zone.UnknownSpoolCount;
            state.LastInventoryTsUtc = message.Ts;
            state.UpdatedAtUtc = now;
        }

        await _db.SaveChangesAsync(cancellationToken);

        var snapshot = await GetSnapshotAsync(message.CameraId, cancellationToken);
        _publisher.Publish(snapshot);
    }

    public async Task UpsertAnomalyMessageAsync(
        CvZoneAnomalyMessageDto message,
        CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;

        foreach (var zoneEntry in message.Zones)
        {
            var zoneName = zoneEntry.Key;
            var zone = zoneEntry.Value;

            var state = await _db.CvZoneStates.FirstOrDefaultAsync(
                x => x.CameraId == message.CameraId && x.ZoneName == zoneName,
                cancellationToken);

            if (state is null)
            {
                state = new CvZoneState
                {
                    Id = Guid.NewGuid(),
                    CameraId = message.CameraId,
                    ZoneName = zoneName,
                    SpoolIdsJson = "[]",
                    CreatedAtUtc = now
                };

                _db.CvZoneStates.Add(state);
            }

            state.OtherObjectCount = zone.OtherCount;
            state.LastAnomalyTsUtc = message.Ts;
            state.UpdatedAtUtc = now;
        }

        await _db.SaveChangesAsync(cancellationToken);

        var snapshot = await GetSnapshotAsync(message.CameraId, cancellationToken);
        _publisher.Publish(snapshot);
    }

    public async Task<ZoneSnapshotResponseDto> GetSnapshotAsync(
        string cameraId,
        CancellationToken cancellationToken = default)
    {
        var states = await _db.CvZoneStates
            .AsNoTracking()
            .Where(x => x.CameraId == cameraId)
            .OrderBy(x => x.ZoneName)
            .ToListAsync(cancellationToken);

        var zoneMap = new Dictionary<string, ZoneViewDto>(StringComparer.OrdinalIgnoreCase);

        foreach (var state in states)
        {
            var spoolIds = DeserializeSpoolIds(state.SpoolIdsJson);
            var colors = await BuildColorCountsAsync(spoolIds, cancellationToken);

            zoneMap[state.ZoneName] = new ZoneViewDto
            {
                Colors = colors,
                UnknownSpoolCount = state.UnknownSpoolCount,
                OtherObjectCount = state.OtherObjectCount,
                UpdatedAtUtc = state.UpdatedAtUtc
            };
        }

        return new ZoneSnapshotResponseDto
        {
            CameraId = cameraId,
            Zones = zoneMap
        };
    }

    public async Task<ZoneSnapshotResponseDto?> GetLatestSnapshotAsync(CancellationToken cancellationToken = default)
    {
        var latestCameraId = await _db.CvZoneStates
            .AsNoTracking()
            .OrderByDescending(x => x.UpdatedAtUtc)
            .Select(x => x.CameraId)
            .FirstOrDefaultAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(latestCameraId))
            return null;

        return await GetSnapshotAsync(latestCameraId, cancellationToken);
    }

    // private async Task<Dictionary<string, int>> BuildColorCountsAsync(
    //     List<string> spoolIds,
    //     CancellationToken cancellationToken)
    // {
    //     var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

    //     if (spoolIds.Count == 0)
    //         return result;

    //     // TODO: replace this with your real spool registry table once available.
    //     // For Phase 1, this assumes you have or will have a Spools table with SpoolId and Color.
    //     // If not available yet, leave colors empty rather than inventing data.
    //     var spoolColorRows = await _db.Set<SpoolRegistryStub>()
    //         .AsNoTracking()
    //         .Where(x => spoolIds.Contains(x.SpoolId))
    //         .Select(x => new { x.SpoolId, x.Color })
    //         .ToListAsync(cancellationToken);

    //     var colorBySpoolId = spoolColorRows
    //         .GroupBy(x => x.SpoolId)
    //         .ToDictionary(g => g.Key, g => g.First().Color ?? "unknown", StringComparer.OrdinalIgnoreCase);

    //     foreach (var spoolId in spoolIds)
    //     {
    //         if (!colorBySpoolId.TryGetValue(spoolId, out var color))
    //             continue;

    //         if (string.IsNullOrWhiteSpace(color))
    //             continue;

    //         result[color] = result.TryGetValue(color, out var count) ? count + 1 : 1;
    //     }

    //     return result;
    // }
    private Task<Dictionary<string, int>> BuildColorCountsAsync(
        List<string> spoolIds,
        CancellationToken cancellationToken)
    {
        var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var spoolId in spoolIds)
        {
            var color = TryExtractColorFromSpoolId(spoolId);
            if (string.IsNullOrWhiteSpace(color))
                continue;

            result[color] = result.TryGetValue(color, out var count) ? count + 1 : 1;
        }

        return Task.FromResult(result);
    }

    private static string? TryExtractColorFromSpoolId(string spoolId)
    {
        if (string.IsNullOrWhiteSpace(spoolId))
            return null;

        // Expected examples:
        // FIL-PETG-RED
        // FIL-PETG-BLUE
        // FIL-PLA-GREEN
        var parts = spoolId
            .Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (parts.Length < 3)
            return null;

        // Treat the last segment as color
        var rawColor = parts[^1];

        if (string.IsNullOrWhiteSpace(rawColor))
            return null;

        return rawColor.ToLowerInvariant();
    }

    private static List<string> DeserializeSpoolIds(string spoolIdsJson)
    {
        if (string.IsNullOrWhiteSpace(spoolIdsJson))
            return new List<string>();

        try
        {
            return JsonSerializer.Deserialize<List<string>>(spoolIdsJson) ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }

    // Temporary stub for compile-time shape. Replace/remove when your real spool registry entity exists.
    private class SpoolRegistryStub
    {
        public Guid Id { get; set; }
        public string SpoolId { get; set; } = default!;
        public string? Color { get; set; }
    }
}