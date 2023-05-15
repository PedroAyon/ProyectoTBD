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
    public class RecurringServicesController : ControllerBase
    {
        private readonly HotelCleanContext _context;

        public RecurringServicesController(HotelCleanContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecurringService>>> GetRecurringservices()
        {
            if (_context.Recurringservices == null)
            {
                return NotFound();
            }
            return await _context.Recurringservices.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecurringService>> GetRecurringService(int id)
        {
            if (_context.Recurringservices == null)
            {
                return NotFound();
            }
            var recurringService = await _context.Recurringservices.FindAsync(id);

            if (recurringService == null)
            {
                return NotFound();
            }

            return recurringService;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecurringService(int id, RecurringService recurringService)
        {
            if (id != recurringService.Id)
            {
                return BadRequest();
            }

            _context.Entry(recurringService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecurringServiceExists(id))
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
        public async Task<ActionResult<RecurringService>> PostRecurringService(RecurringService recurringService)
        {
            if (_context.Recurringservices == null)
            {
                return Problem("Entity set 'HotelCleanContext.Recurringservices'  is null.");
            }
            _context.Recurringservices.Add(recurringService);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecurringService", new { id = recurringService.Id }, recurringService);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecurringService(int id)
        {
            if (_context.Recurringservices == null)
            {
                return NotFound();
            }
            var recurringService = await _context.Recurringservices.FindAsync(id);
            if (recurringService == null)
            {
                return NotFound();
            }

            _context.Recurringservices.Remove(recurringService);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecurringServiceExists(int id)
        {
            return (_context.Recurringservices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
