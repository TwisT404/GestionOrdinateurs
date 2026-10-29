using GestionOrdinateurs.API.Data;
using GestionOrdinateurs.API.JournalService;
using GestionOrdinateurs.Shared;
using Microsoft.AspNetCore.Mvc;
using GestionOrdinateurs.API.Services;
using Microsoft.EntityFrameworkCore;

public class UsersService : IUsersService
{
    //Propriétés
    public ParcInformatiqueContext _context; //Lien avec la db
    private JournalService _journal; //Logs

    public UsersService(ParcInformatiqueContext context)
    {
        _context = context;
        _journal = new JournalService();
    }

    public UsersService()
    {
        var options = new DbContextOptionsBuilder<ParcInformatiqueContext>()
            .Options;

        _context = new ParcInformatiqueContext(options);
        _journal = new JournalService();
    }

    public async Task<List<User>> AfficherUtilisateurs()
    {
        return await _context.Utilisateurs.ToListAsync();
    }

    public async Task<User?> AfficherUtilisateurParLogin(string login)
    {
        return await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Login == login);
    }

}