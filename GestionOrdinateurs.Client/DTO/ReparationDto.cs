using GestionOrdinateurs.Shared;
public record ReparationDto(Ordinateurs UnOrdinateur, string TypeReparation, DateTime DateDebut, string statut, string Technicien);