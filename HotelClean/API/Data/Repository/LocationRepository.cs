using API.Data.Interfaces;
using API.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly HotelCleanContext _context;

        public LocationRepository(HotelCleanContext context)
        {
            _context = context;
        }

        public async Task<Location> AddLocation(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return location;

        }

        public async Task DeleteLocation(Location location)
        {
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
        }

        public async Task<Location> GetLocation(int id) => await _context.Locations.FindAsync(id);


        public async Task<IEnumerable<Location>> GetLocations() => await _context.Locations.ToListAsync();

        public async Task<bool> LocationExists(int id) => await _context.Locations.AnyAsync(l => l.Id == id);

        public async Task UpdateLocation(Location location)
        {
            _context.Entry(location).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
