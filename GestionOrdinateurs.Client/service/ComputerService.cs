using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.SessionStorage;
using Blazored.SessionStorage.StorageOptions;
using GestionOrdinateurs.Shared;

public class ComputerService : IComputerService
{
    //Attributs
    private readonly HttpClient _httpClient;
    private readonly ISessionStorageService _session;
    
    //Constructeur
    public ComputerService(HttpClient httpClient, ISessionStorageService session)
    {
        _httpClient = httpClient;
        _session = session;
    }

    public async Task<List<ComputerDto>?> GetComputers()
    {
        //Récupére le token
        var token = await _session.GetItemAsync<string>("jwtToken");

        //Requete pour recuperer les données
        using var request = new HttpRequestMessage(HttpMethod.Get, "api/ordinateurs");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        //Donner accès à la requete
        var httpResponse = await _httpClient.SendAsync(request);
        httpResponse.EnsureSuccessStatusCode();

        var response = await httpResponse.Content.ReadFromJsonAsync<List<ComputerDto>>();
        if (response is null || response.Count == 0)
        {
            return null;
        }

        return response;
    }
}