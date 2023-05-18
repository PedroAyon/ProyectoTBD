using System;
using System.Data;
using API.Data.Interfaces;
using API.Domain.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace API.Data.Repository
{
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelCleanContext _context;

        public HotelRepository(HotelCleanContext context)
        {
            _context = context;
        }

        public async Task AddHotel(Hotel hotel) 
        {
            var hotelExists = await _context.Hotels.AnyAsync(x => x.Name == hotel.Name);
            if (!hotelExists)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"call createHotel({hotel.Name})");
            }
            else
            {
                throw new ValidationError($"{hotel.Name} ya existe.");
            }
        }

        public async Task<IEnumerable<Hotel>> GetAllHotels() => await _context.Hotels.ToListAsync();

        public async Task<Hotel> GetHotelById(int id) => await _context.Hotels.FindAsync(id);
    }
}
