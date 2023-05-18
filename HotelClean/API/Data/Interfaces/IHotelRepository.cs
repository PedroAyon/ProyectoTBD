using API.Domain.Model;

namespace API.Data.Interfaces
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAllHotels();
        Task<Hotel> GetHotelById(int id);
        Task AddHotel(Hotel hotel) ;
    }
}
