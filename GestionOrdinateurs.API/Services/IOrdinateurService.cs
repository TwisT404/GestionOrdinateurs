using GestionOrdinateurs.Shared;
namespace GestionOrdinateurs.API.Services;

//Interface
public interface IOrdinateurService
{

    //Méthodes signatures
    public Task ajouterOrdinateur(Ordinateurs ordi);
    public Task supprimerOrdinateur(int id);
    public void modifierOrdinateur(int id, string nom, string marque, int stock, int prix);
    public Task<List<Ordinateurs>> lireOrdinateurs();
    public Task<Ordinateurs?> lireOrdinateur(int id);
    public Task<List<Ordinateurs>> lireOrdinateursEnStock();
    public Task<List<Ordinateurs>> afficherOrdinateursFiltreParMarque(string marque);
    public Task<List<Ordinateurs>> afficherOrdinateursParPrixDécroissant();
    public Task<string> Diagnostiquer(Ordinateurs ordinateur);
    public (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations) CalculerStatistiques();


}