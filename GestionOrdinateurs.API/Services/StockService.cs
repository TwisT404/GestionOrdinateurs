public class StockService
{
    //Propriétés
    public int StockSSD = 10;
    static readonly object stockLock = new object();
    
    //Consommation du SSD avec un competeur qui décrémente
    public async Task ConsommerSSD()
    {   
        //Verrou pour le changement
        lock (stockLock)
        {
            Task.Delay(10).Wait();
            this.StockSSD--;
        }

    }
}