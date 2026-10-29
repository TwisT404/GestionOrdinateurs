public interface IReparationService
{
    public Task<List<ReparationDto>?> GetReparations();
}