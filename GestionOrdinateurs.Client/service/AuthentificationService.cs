using System.Net.Http.Json;

public class AuthentificationService : IAuthentificationService
{
    //Attributs
    private readonly HttpClient _httpClient;
    
    //Constructeur
    public AuthentificationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    
    //Requête d'authentification par le login
    public async Task<string> LogAsync(string login)
    {

        var response = await _httpClient.PostAsync($"api/Authentification?login={Uri.EscapeDataString(login)}", null);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }else
        {
            return string.Empty;
        }
    }
}