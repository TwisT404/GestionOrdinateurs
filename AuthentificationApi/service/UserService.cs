public class UserService : IUserService
{
    //Attributs
    private readonly HttpClient _httpClient;

    //Constructeur
    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    //Récupère l'utilisateur par son login depuis l'API
    public async Task<UserDto?> GetUserAsync(string login)
    {
        return await _httpClient.GetFromJsonAsync<UserDto> ($"api/users/search/{login}") ?? null;

    }

}