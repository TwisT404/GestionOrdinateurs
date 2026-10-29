namespace GestionOrdinateurs.Shared;

public class InventaireOrdinateurs<Ordinateurs>
{
    public Ordinateurs[] arrOrdi = new Ordinateurs[100];
    public Dictionary<string, Ordinateurs> dicoOrdis = new();

    public Ordinateurs? this[string reference]
    {
        get{
            return dicoOrdis.TryGetValue(reference, out var value) ? value : default;
        }
        
        set
        {
            dicoOrdis[reference] = value;
        }
    }

    public Ordinateurs this[int index]
    {
        get
        {
            return arrOrdi[index];
        }
        set
        {
            arrOrdi[index] = value;
        }
    }

    public void Ajouter(int index, string reference, Ordinateurs ordi)
    {
        arrOrdi[index] = ordi;
        dicoOrdis[reference] = ordi;
    }

}