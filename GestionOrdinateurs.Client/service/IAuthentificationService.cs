public interface IAuthentificationService
{
    public Task<string> LogAsync(string login, string password);
}