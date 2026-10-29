using Microsoft.AspNetCore.Mvc;
using GestionOrdinateurs.Shared;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/")]
public class ReparationController : ControllerBase
{
    //Propriétés
    private readonly ReparationService _service = new ReparationService();
    private readonly StockService _stockService = new StockService();
    private readonly ILogger<ReparationController> _logger; //logger
    private readonly IConfiguration _configuration; //Configuration
    
    //Constructeur
    public ReparationController(ILogger<ReparationController> logger, IConfiguration config)
    {
        _configuration = config;
        _logger = logger;
    }
    
    //Endpoint pour test la mise en place des ordinateurs
    [Authorize(Roles = "Admin")]
    [HttpPost("reparations/test")]
    public async Task<IActionResult> SimulerReparation()
    {
        
        List<Ordinateurs> listOrdinateurs = new List<Ordinateurs>
        {
            new Ordinateurs("Latitude 5520", "Dell", EtatOrdinateur.EnMaintenance, 25, 1200, new Caracteristiques(20, TypeOrdinateur.Portable, 150)),
            new Ordinateurs("ThinkPad X1 Carbon", "Lenovo", EtatOrdinateur.EnService, 10, 1500, new Caracteristiques(10, TypeOrdinateur.Server, 100)),
            new Ordinateurs("MacBook Pro 14", "Apple", EtatOrdinateur.EnPanne, 5, 1800, new Caracteristiques(30, TypeOrdinateur.Fixe, 200)),
            new Ordinateurs("ThinkPad X1 Carbon", "Lenovo", EtatOrdinateur.HorsService, 10, 1500, new Caracteristiques(10, TypeOrdinateur.Server, 100)),
            new Ordinateurs("MacBook Pro 14", "Apple", EtatOrdinateur.EnService, 5, 1800, new Caracteristiques(30, TypeOrdinateur.Fixe, 200))
        };

        //Récupération de la liste d'ordinateurs.
        foreach (var ordinateur in listOrdinateurs) { 
            await _service.SimulerReparationAsync(ordinateur); 
        }

        return Ok(listOrdinateurs.Select(o => new
        {
            o.Nom,
            o.Marque,
            o.Etat,
            o.Stock,
            o.Prix
        }));
           
    }
    
    //Affiche les réparations
    [Authorize(Roles = "Admin")]
    [HttpGet("/reparations")]
    public async Task<IActionResult> Get()
    {
        List<Reparation> listeReparations = await _service.AfficherReparations();
        return Ok(listeReparations);
    }

    //Affiche le stock
    [Authorize(Roles = "Admin")]
    [HttpGet("/reparations/logStock")]
    public async Task<IActionResult> GetSSD()
    {
        Parallel.For(
            0,
            10,
            i => _stockService.ConsommerSSD()
        );
        return Ok(_stockService.StockSSD);
    }
    
    //Récupère les réparations du record
    [Authorize(Roles = "Admin")]
    [HttpGet("/reparations/historique")]
    public async Task<IActionResult> GetRecord()
    {
        return Ok(await _service.AfficherListeHistoriqueReparation());
    }
}