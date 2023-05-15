using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Domain.Model;
using API.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly HotelCleanContext _context;

        public LocationsController(HotelCleanContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            if (_context.Locations == null)
            {
                return NotFound();
            }
            return await _context.Locations.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            if (_context.Locations == null)
            {
                return NotFound();
            }
            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            return location;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }

            _context.Entry(location).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(Location location)
        {
            if (_context.Locations == null)
            {
                return Problem("Entity set 'HotelCleanContext.Locations'  is null.");
            }
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocation", new { id = location.Id }, location);
        }


        private bool LocationExists(int id)
        {
            return (_context.Locations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
