using Client.Domain.Model;

namespace Client.Data.Interfaces
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetLocations();
        Task AddLocation(Location location);
        Task UpdateLocation(Location location);
    }
}
