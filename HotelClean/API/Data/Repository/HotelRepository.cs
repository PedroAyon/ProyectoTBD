using API.Data.Interfaces;
using API.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelCleanContext _context;

        public HotelRepository(HotelCleanContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hotel>> GetAllHotels() => await _context.Hotels.ToListAsync();

        public async Task<Hotel> GetHotelById(int id) => await _context.Hotels.FindAsync(id);
    }
}
