using System.Net.Http.Json;
using DigitalTwin.Application.Abstractions.External;
using DigitalTwin.Application.Printers.Dtos;

namespace DigitalTwin.Infrastructure.External.BambuProxy;

public class BambuProxyClient : IBambuProxyClient
{
    private readonly HttpClient _httpClient;

    public BambuProxyClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BindResponseDto> GetBindAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<BindResponseDto>(
            "/v1/iot-service/api/user/bind",
            cancellationToken);

        return response ?? new BindResponseDto();
    }

    public async Task<VersionResponseDto> GetVersionAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<VersionResponseDto>(
            "/v1/iot-service/api/user/device/version",
            cancellationToken);

        return response ?? new VersionResponseDto();
    }
}