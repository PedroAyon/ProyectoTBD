using API.Domain.Model;

namespace API.Data.Interfaces
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetLocations();
        Task<Location> GetLocation(int id);
        Task<Location> AddLocation(Location location);
        Task UpdateLocation(Location location);
        Task DeleteLocation(Location location);
        Task<bool> LocationExists(int id);
    }
}
