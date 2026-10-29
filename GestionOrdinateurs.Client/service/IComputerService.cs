using GestionOrdinateurs.Shared;

public interface IComputerService
{
    public Task<List<ComputerDto>?> GetComputers();
}