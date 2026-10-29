using GestionOrdinateurs.Shared;
using GestionOrdinateurs.API.Services;
using Microsoft.EntityFrameworkCore;
using GestionOrdinateurs.API.Data;
using GestionOrdinateurs.API.JournalService;


public class OrdinateursService : IOrdinateurService
{
    //Propriétés
    public ParcInformatiqueContext _context; //Lien avec la db
    private JournalService _journal; //Logs

    
    //Events pour l'ajout, suppresion et modification
    public event EventHandler<OrdinateurAjouteEventArgs> OrdinateurAjoute;
    public event EventHandler<OrdinateurSupprimeEventArgs> OrdinateurSupprime;
    public event EventHandler<OrdinateurMisAJourEventArgs> OrdinateurMAJ;
    
    //Constructeur
    public OrdinateursService(ParcInformatiqueContext context)
    {
        _context = context;
        _journal = new JournalService();

    }

    public OrdinateursService()
    {
        
        var options = new DbContextOptionsBuilder<ParcInformatiqueContext>()
            .Options;

        _context = new ParcInformatiqueContext(options);
        _journal = new JournalService();

    }
    
    //Ajouter un ordinateur dans la db
    public async Task ajouterOrdinateur(Ordinateurs ordi)
    {
        await _context.Ordinateurs.AddAsync(ordi);
        _context.SaveChangesAsync();
        OrdinateurAjoute?.Invoke(this, new OrdinateurAjouteEventArgs{Ordinateur = ordi});
        _journal.Ecrire($"Ajout au {DateTime.Now} du {ordi.Marque} de {ordi.Nom} ");
    }
    
    //Supprimer un ordi de la db
    public async Task supprimerOrdinateur(int id)
    {
        Ordinateurs? ordi = await lireOrdinateur(id);
        if (ordi == null)
        {
            return;
        }
        _context.Ordinateurs.Remove(ordi);
        await _context.SaveChangesAsync();
        OrdinateurSupprime?.Invoke(this, new OrdinateurSupprimeEventArgs{});
        _journal.Ecrire($"Suppresion au {DateTime.Now} du {ordi.Marque} de {ordi.Nom} ");
    }
    
    //Modifier un ordinateur de la db
    public async void modifierOrdinateur(int id, string nom, string marque, int stock, int prix)
    {
        Ordinateurs? ordi = await lireOrdinateur(id);
        if (ordi == null)
        {
            return;
        }
        ordi.Nom = nom;
        ordi.Marque = marque;
        ordi.Stock = stock;
        ordi.Prix = prix;
        await _context.SaveChangesAsync();
        OrdinateurMAJ?.Invoke(this, new OrdinateurMisAJourEventArgs{Ordinateur = ordi});
        
        
    }
    
    //Lire l'ensemble des ordis de la db
    public async Task<List<Ordinateurs>> lireOrdinateurs()
    {
        // return ordis;
        return await _context.Ordinateurs.ToListAsync();
    }
    
    //Lire un ordi de la db
    public async Task<Ordinateurs?> lireOrdinateur(int id)
    {
        return await _context.Ordinateurs.FindAsync(id);
    }
    
    //Affiche les ordinateurs qui sont en stock
    public async Task<List<Ordinateurs>> lireOrdinateursEnStock()
    {
        return await _context.Ordinateurs
            .Where(o => o.Stock > 0)
            .ToListAsync();
    }

    //Afficher les ordinateurs filtré
    public async Task<List<Ordinateurs>> afficherOrdinateursFiltreParMarque(string marque)
    {
        return await _context.Ordinateurs
            .Where(o => o.Marque == marque)
            .ToListAsync();
    }
    
    //Affiche les ordinateurs par prix décroisssant
    public async Task<List<Ordinateurs>> afficherOrdinateursParPrixDécroissant()
    {
        return await _context.Ordinateurs
            .OrderByDescending(o => o.Prix)
            .ToListAsync();
    }

    //Diagnostique l'etat d'un ordinateur
    public async Task<string> Diagnostiquer(Ordinateurs ordinateur)
    {
        _journal.Ecrire($"Diagnostic au {DateTime.Now} : {ordinateur.Marque} de {ordinateur.Nom}");
        var ordinateurResultat = ordinateur;
        return ordinateurResultat switch
        {
            {
                Etat: EtatOrdinateur.HorsService
            }
                => "Remplacement",

            {
                Etat: EtatOrdinateur.Lent,
                Caracteristiques:
                {
                    RamGo: < 8
                }
            }
                => "Ajouter RAM",
            //-------Default--------//
            _ => "Aucune action"

        };

    }
    
    //Fonction Tuple
    public (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations) CalculerStatistiques()
    {
        var totalOrdinateur = _context.Ordinateurs.Count();
        var ordiDisponibles = _context.Ordinateurs.Count(o => o.Stock > 0);
        var valeurStock = _context.Ordinateurs
            .Where(o => o.Stock > 0)
            .Sum(o => o.Prix * o.Stock);
        var nbReparations = _context.Ordinateurs
            .Count(o => o.Reparations.Any(r => r.Statut == "Terminé"));

        return (totalOrdinateur, ordiDisponibles, valeurStock, nbReparations);
    }

}