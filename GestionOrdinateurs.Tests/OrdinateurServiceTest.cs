using GestionOrdinateurs.Shared;
using GestionOrdinateurs.API.Services;
using GestionOrdinateurs.API.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace GestionOrdinateurs.Tests;



public class OrdinateurServiceTest
{
    
    // Création de contexte pour les tests
    private ParcInformatiqueContext CreerContexte()
    {
        var options =
            new DbContextOptionsBuilder<ParcInformatiqueContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        return new ParcInformatiqueContext(options);
    }

    //Vérification de l'ajout d'un ordinateur
    [Fact]
    public async Task AjouterOrdi_VerifierAjout_AjouterNombre()
    {
        // Given
        var contexte = CreerContexte();
        OrdinateursService _service = new OrdinateursService(contexte);
        var caracteristiques = new Caracteristiques(20, TypeOrdinateur.Portable, 150);
        var ordi = new Ordinateurs("Tanghe", "Asus", EtatOrdinateur.EnService, 100, 1990, caracteristiques);
        
        // When
        await _service.ajouterOrdinateur(ordi);

        // Then
        Assert.Contains(ordi, await _service.lireOrdinateurs());
    }

    //Vérification la suppresion d'un ordinateur
    [Fact]
    public async Task SupprimerOrdi_VerifierSuppression_DiminuerNombre()
    {
        // Given
        var contexte = CreerContexte();
        OrdinateursService _service = new OrdinateursService(contexte);
        
        var characteristiques1 =  new Caracteristiques(20, TypeOrdinateur.Portable, 150);
        var ordi1 = new Ordinateurs("Tanghe", "Asus", EtatOrdinateur.EnService, 100, 1990, characteristiques1);
        await _service.ajouterOrdinateur(ordi1);
        var characteristiques2 = new Caracteristiques(10, TypeOrdinateur.Server, 100);
        var ordi2 = new Ordinateurs("Cedric", "Dell", EtatOrdinateur.EnService, 15, 5000, characteristiques2);
        await _service.ajouterOrdinateur(ordi2);
        

        // When
        await _service.supprimerOrdinateur(ordi2.Id);
        List<Ordinateurs> ordis = await _service.lireOrdinateurs();

        // Then
        Assert.DoesNotContain(ordi2, ordis);
    }

    
    //Vérifie bien que les réparations soient bien efffectuées
    [Fact]
    public async Task RéparationOrdi_VerifierReparation_StatutTerminer()
    {
        // Given
        var contexte = CreerContexte();
        OrdinateursService _serviceOrdi = new OrdinateursService(contexte);
        ReparationService _serviceRepa = new ReparationService(contexte);
        var characteristiques1 =  new Caracteristiques(20, TypeOrdinateur.Portable, 150);
        var ordi1 = new Ordinateurs("Tanghe", "Asus", EtatOrdinateur.EnPanne, 100, 1990, characteristiques1);
        // When
        await _serviceOrdi.ajouterOrdinateur(ordi1);   

        // When
        await _serviceRepa.SimulerReparationAsync(ordi1);

        // Then
        var reparation = await contexte.Reparations
        .FirstOrDefaultAsync(r => r.OrdinateurId == ordi1.Id);

        Assert.Equal("Terminée", reparation?.Statut);
    }
}
