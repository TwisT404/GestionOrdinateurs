using System.Net.Http.Json;
using System.Text.Json.Serialization;

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
    public async Task<string> LogAsync(string login, string password)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "api/KeycloakAuth/login",
            new LoginRequest { Username = login, Password = password });

        if (!response.IsSuccessStatusCode)
            return string.Empty;

        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return result?.AccessToken ?? string.Empty;
    }

    private sealed class LoginRequest
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }

    public class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}
}