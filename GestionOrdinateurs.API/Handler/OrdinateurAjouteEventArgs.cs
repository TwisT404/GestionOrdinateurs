using GestionOrdinateurs.Shared;

//Event d'ajout
public class OrdinateurAjouteEventArgs : EventArgs
{
    public Ordinateurs Ordinateur {get;set;}
}