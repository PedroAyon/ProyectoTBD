using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Domain.Model;
using API.Data.Repository;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly HotelRepository _hotelRepository;

        public HotelsController(HotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            var hotels = await _hotelRepository.GetAllHotels();
            if (hotels == null)
            {
                return NotFound();
            }
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {

            var hotel = await _hotelRepository.GetHotelById(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return hotel;
        }
    }
}
