using GestionOrdinateurs.Shared;
using Microsoft.AspNetCore.Mvc;
using GestionOrdinateurs.API.Services;
using GestionOrdinateurs.API.Data;
using Microsoft.AspNetCore.Authorization;


namespace GestionOrdinateurs.Controllers;

[ApiController]
[Route("api/")]
public class OrdinateursController : ControllerBase
{
    //Propriétés
    private readonly IOrdinateurService _service = new OrdinateursService();
    private readonly ILogger<OrdinateursController> _logger; //logger
    private readonly IConfiguration _configuration; //Configuration
    
    public OrdinateursController(ILogger<OrdinateursController> logger, IConfiguration config)
    {
        _configuration = config;
        _logger = logger;
    }
    
    
    //Détail sur les infos de l'application
    [Authorize(Roles = "Admin")]
    [HttpGet("api/ordinateurs/info")]
    public IActionResult Info()
    {
        var nomApplication = _configuration["Application:Name"];
        return Ok(nomApplication);
        
    }
    
    
    //Afficher un ordi en particulier
    [Authorize(Roles = "Admin")]
    [HttpGet("ordinateur/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        Ordinateurs? ordi = await _service.lireOrdinateur(id);
        if (ordi == null)
        {
            _logger.LogInformation($"Problème d'ajout !");
            return NotFound($"Ordinateur {id} non trouvé !");
        }
        _logger.LogInformation($"Ordinateur {id} ajouté !");
        return Ok(ordi);
        
        
    }
    
    //Afficher l'ensemble des ordis de la listes
    [Authorize(Roles = "Admin")]
    [HttpGet("ordinateurs")]
    public async Task<IActionResult> Get()
    {
        List<Ordinateurs> ordis = await _service.lireOrdinateurs();
        if (ordis.Count == 0)
        {
            return NotFound($"Aucun ordinateur trouvé !");
        }
        _logger.LogInformation($"Ordinateurs trouvés !");
        return Ok(ordis);
    }

    //Afficher l'ensemble des ordis de la listes ayant un stock > 0
    [Authorize(Roles = "Admin")]
    [HttpGet("ordinateurs/stockExistant")]
    public async Task<IActionResult> GetFilterByStock()
    {
        List<Ordinateurs> ordis = await _service.lireOrdinateursEnStock();
        if (ordis.Count == 0)
        {
            return NotFound($"Aucun ordinateur trouvé !");
        }
        _logger.LogInformation($"Ordinateurs trouvés !");
        return Ok(ordis);
    }
    
    
    //Supprimer un ordi de la liste basé sur son id
    [Authorize(Roles = "Admin")]
    [HttpDelete("supprimer/{id}")]
    public async Task<IActionResult> Del(int id)
    {
        Ordinateurs? ordi =  await _service.lireOrdinateur(id);
        if (ordi == null)
        {
            return NotFound($"Ordinateur {id} non trouvé !");
        }
        await _service.supprimerOrdinateur(id);
        _logger.LogInformation($"Ordinateur {id} bien supprimé !");
        AfficherMessageSuppression(_service, new OrdinateurSupprimeEventArgs {});
        return Ok();
    }
    
    //Ajouter un ordi à la liste
    [Authorize(Roles = "Admin")]
    [HttpPost("ordinateurs/ajouter")]
    public async Task<IActionResult> Add(string nom, string marque, int stock, int prix, Caracteristiques caracteristiques)
    {
        Ordinateurs ordi = new Ordinateurs(nom, marque, EtatOrdinateur.EnService, stock, prix, caracteristiques);
        await _service.ajouterOrdinateur(ordi);
        _logger.LogInformation($"Ordinateur {ordi.Id} bien ajouté !");
        AfficherMessageAjout(_service, new OrdinateurAjouteEventArgs { Ordinateur = ordi });
        return Ok();
    }
    
    //Modifier un ordi existant de la liste 
    [Authorize(Roles = "Admin")]
    [HttpPut("ordinateurs/modifier/{id}")]
    public async Task<IActionResult> Update(int id, string nom, string marque, int stock, int prix)
    {
        Ordinateurs? ordi = await _service.lireOrdinateur(id);
        if (ordi == null)
        {
            return NotFound($"Ordinateur {id} non trouvé !");
        }

        _service.modifierOrdinateur(
            id, 
            nom == null ? ordi.Nom : nom, 
            marque == null ? ordi.Marque : marque , 
            stock != 0 ? ordi.Stock : stock,
            prix != 0 ? ordi.Prix : prix
        );

        _logger.LogInformation($"Ordinateur {id} bien modifié !");
        AfficherMessageModif(_service, new OrdinateurMisAJourEventArgs{Ordinateur = ordi});
        return Ok();
        }
    
    //Affiche les ordinateurs d'une certaine marque
    [Authorize(Roles = "Admin")]
    [HttpGet("ordinateurs/rechercheParMarque/{marque}")]
    public async Task<IActionResult> GetByBrand(string marque)
    {
        List<Ordinateurs> ordis = await _service.afficherOrdinateursFiltreParMarque(marque);
        if (ordis.Count == 0)
        {
            return NotFound($"Aucun ordinateur trouvé !");
        }
        _logger.LogInformation($"Ordinateurs trouvés !");
        return Ok(ordis);
    }
    
    //Affiche les orddianteurs dans un ordre de prix décroissant
    [Authorize(Roles = "Admin")]
    [HttpGet("ordinateurs/PrixDécroissant/")]
    public async Task<IActionResult> GetByPriceDescending()
    {
        List<Ordinateurs> ordis = await _service.afficherOrdinateursParPrixDécroissant();
        if (ordis.Count == 0)
        {
            return NotFound($"Aucun ordinateur trouvé !");
        }
        _logger.LogInformation($"Ordinateurs trouvés !");
        return Ok(ordis);
    }

    //Endpoint pour faire un diagnostic
    [Authorize(Roles = "Admin")]
    [HttpGet("ordinateurs/{id}/diagnostic")]
    public async Task<IActionResult> GetDiagnostic(int id)
    {
        var ordi = await _service.lireOrdinateur(id);
        if (ordi == null)
        {
            return NotFound();
        }
        
        return Ok(_service.Diagnostiquer(ordi));
    }

    
    //Endpoint pour voir les stats
    [Authorize(Roles = "Admin")]
    [HttpGet("ordinateurs/stats")]
    public async Task<IActionResult> getStats()
    {
        var (total, enStock, valStock, nbReparations) = _service.CalculerStatistiques();
        return Ok((total,enStock,valStock,nbReparations));

    }
    
    //Endpoint pour voir l'inventaire par rapport à un nom
    [Authorize(Roles = "Admin")]
    [HttpGet("inventaire/{nom}")]
    public async Task<IActionResult> Chercher(string nom)
    {
        var inventaire = new InventaireOrdinateurs<Ordinateurs>();
        var ordinateurs = await _service.lireOrdinateurs();

        inventaire.dicoOrdis = new Dictionary<string, Ordinateurs>();
        foreach (var ordi in ordinateurs)
        {
            inventaire.dicoOrdis[ordi.Nom] = ordi;
        }

        var ordinateur = inventaire[nom]; // Exemple de recherche par nom

        return Ok(ordinateur);
    }


    //Message d'ajout
    void AfficherMessageAjout(object? sender,OrdinateurAjouteEventArgs e)
    {
        Console.WriteLine(
        $"Nouvel ordinateur : {e.Ordinateur.Id}");
    }

    //Message de suppression
    void AfficherMessageSuppression(object? sender, OrdinateurSupprimeEventArgs e)
    {
        Console.WriteLine(
        $"Ordinateur supprimé !");
    }
    
    //Message de modification 
    void AfficherMessageModif(object? sender,  OrdinateurMisAJourEventArgs e)
    {
        Console.WriteLine($"Ordinateur {e.Ordinateur.Id} modifié !");
    }

}