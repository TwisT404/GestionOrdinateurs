public interface IUserService
{
    public Task<UserDto?> GetUserAsync(string login);
}