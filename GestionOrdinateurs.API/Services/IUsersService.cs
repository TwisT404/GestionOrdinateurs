using GestionOrdinateurs.Shared;

public interface IUsersService
{
    public Task<List<User>> AfficherUtilisateurs();
    public Task<User?> AfficherUtilisateurParLogin(string login);
}