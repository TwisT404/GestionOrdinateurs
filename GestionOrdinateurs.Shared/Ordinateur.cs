using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace GestionOrdinateurs.Shared;

public class Ordinateurs
{
    //Propriétés
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Marque { get; set; }
    public EtatOrdinateur Etat { get; set; }
    [DefaultValue(0)]
    public int Stock { get; set; }
    [DefaultValue(0)]
    public int Prix { get; set; }
    public List<Reparation> Reparations { get; set; } = new();
    public Caracteristiques Caracteristiques { get; set; }
    public bool Vendu {get;set;}

    // EF Core utilise ce constructeur pour matérialiser l'entité, puis renseigne ses propriétés.
    private Ordinateurs()
    {
    }

    //Constructeur
    public Ordinateurs(string nom, string marque, EtatOrdinateur etat, int stock, int prix, Caracteristiques caracteristiques) 
    {
        Nom = nom;
        Marque = marque;
        Etat = etat;
        Stock = stock;
        Prix = prix;
        Caracteristiques = caracteristiques;
    }

    
    public virtual string ToString()
    {
        return $"Ordinateur {Id} :" +
        $"Nom : {Nom}" +
        $"Marque : {Marque}" +
        $"Etat du Processeur : {Etat}"+
        $"Stock : {Stock}";
    }
    
    //Affiche un message pour montrer l'appartenance de l'ordinateur
    public virtual string OrdinateurAppartenant()
    {
        return $"L'ordinateur {Id} appartient à {Nom}";
    }




}
