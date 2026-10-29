using GestionOrdinateurs.Shared;
using GestionOrdinateurs.API.Services;
using Microsoft.EntityFrameworkCore;
using GestionOrdinateurs.API.Data;
using GestionOrdinateurs.Shared;
using GestionOrdinateurs.API.JournalService;

public class ReparationService
{
    public ParcInformatiqueContext _context; //Lien avec la db
    private JournalService _journal; //Logs
    
    //Constructeur avec context
    public ReparationService(ParcInformatiqueContext context)
    {
        _context = context;
        _journal = new JournalService();

    }
    
    //Constructeur sans contexte
    public ReparationService()
    {
        
        var options = new DbContextOptionsBuilder<ParcInformatiqueContext>()
            .Options;

        _context = new ParcInformatiqueContext(options);
        _journal = new JournalService();

    }
    
    //record
    public record HistoriqueReparationRecord(
        int OrdinateurId,
        string NomOrdinateur,
        string Technicien,
        string TypeReparation,
        DateTime DateIntervention
    );
    
    //Affiche les record des réparations
    public async Task<List<HistoriqueReparationRecord>> AfficherListeHistoriqueReparation()
    {
        return await _context.Reparations
        .Select(r => new HistoriqueReparationRecord(
            r.OrdinateurId,
            r.UnOrdinateur.Nom,
            r.Technicien,
            r.TypeReparation,
            r.DateDebut
        ))
        .ToListAsync();
    }
 
    
    
    //Fonction pour simuler la réparation
    public async Task SimulerReparationAsync(Ordinateurs ordinateur)
    {
        Reparation test = new Reparation(
            ordinateur.Id,
            ordinateur,
            "Maintenance", 
            DateTime.Now, 
            "En cours",
            "Eric"
        );
        
        _context.Reparations.Add(test);
        await _context.SaveChangesAsync();
        _journal.Ecrire($"{DateTime.Now} - {ordinateur.Marque} réparé");

        //Méthode de création d'une réparation en base
        await Task.Delay(2000);
        test.Statut = "Terminée";
        await _context.SaveChangesAsync();
        Console.WriteLine(
            $"{ordinateur.Nom} réparé");
    }
    
    public async Task<List<Reparation>> AfficherReparations()
    {
        return await _context.Reparations.ToListAsync();
    }
}