using System.ComponentModel.DataAnnotations;
using GestionOrdinateurs.Shared;
namespace GestionOrdinateurs.Shared;

public class Reparation
{
    //Propriétés
    public int Id { get; set; }
    public int OrdinateurId { get; set; } //Clé étrangère

    public Ordinateurs UnOrdinateur { get; set; }

    public string TypeReparation { get; set; }

    public DateTime DateDebut { get; set; }

    public string Statut { get; set; }
    public string Technicien {get;set;}

    //Constructeur avec paramètres
    public Reparation(int OrdinateurId, Ordinateurs UnOrdinateur, string typeReparation, DateTime dateDebut, string statut, string technicien)
    {
        this.OrdinateurId = OrdinateurId;
        this.UnOrdinateur = UnOrdinateur;
        this.TypeReparation = typeReparation;
        this.DateDebut = dateDebut;
        this.Statut = statut;
        this.Technicien = technicien;

    }
    //Constructeur sans paramètre
    public Reparation(){}
}