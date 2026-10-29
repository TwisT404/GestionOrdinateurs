namespace GestionOrdinateurs.API.JournalService;
public class JournalService
{
    //Logs
    public void Ecrire(string message)
    {
        using var writer =
            new StreamWriter(
                "reparations.log",
                append: true);

        writer.WriteLine(message);
    }
}