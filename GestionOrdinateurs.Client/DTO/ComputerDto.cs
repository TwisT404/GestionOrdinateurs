using GestionOrdinateurs.Shared;
public record ComputerDto(string Nom, string Marque, string Etat, int Stock,int Prix, List<Reparation> Reparations,Caracteristiques Caracteristiques, bool Vendu);