using Blazored.SessionStorage;
using GestionOrdinateurs.Shared;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class ReparationService : IReparationService
{
    private readonly HttpClient _httpClient;
    private readonly ISessionStorageService _session;
    public ReparationService(HttpClient httpClient, ISessionStorageService session)
    {
        _httpClient = httpClient;
        _session = session;
    }

    public async Task<List<ReparationDto>?> GetReparations()
    {
        var token = await _session.GetItemAsync<string>("jwtToken");
        using var request = new HttpRequestMessage(HttpMethod.Get, "api/ordinateurs");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var httpResponse = await _httpClient.SendAsync(request);
        var response = await _httpClient.GetFromJsonAsync<List<ReparationDto>>("api/reparations");
        if (response is null || response.Count == 0)
        {
            return null;
        }

        return response;
    }
}