using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Domain.Model;
using API.Data;
using API.Data.Interfaces;
using API.Data.Repository;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationRepository locationRepository;
        private readonly IEmployeeRepository employeeRepository;

        public LocationsController(ILocationRepository locationRepository, IEmployeeRepository employeeRepository)
        {
            this.locationRepository = locationRepository;
            this.employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations([FromQuery] Credentials credentials)
        {
            var locations = await locationRepository.GetLocations()!;
            if (locations == null) return NotFound();
            return Ok(locations);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, LocationParameterAuthenticated locationParameterAuthenticated)
        {
            var credentials = locationParameterAuthenticated.credentials;
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null || employee.Position != "Administracion")
                return BadRequest("No autorizado");
            var location = locationParameterAuthenticated.location;
            if (id != location.Id) return BadRequest();
            await locationRepository.UpdateLocation(location);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(LocationParameterAuthenticated locationParameterAuthenticated)
        {
            var credentials = locationParameterAuthenticated.credentials;
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null || employee.Position != "Administracion")
                return BadRequest("No autorizado");
            var location = locationParameterAuthenticated.location;
            if (location.Name == null && location.Number == null)
                return BadRequest("No puede tener nombre y numero a la vez");
            if (location.Number != null && location.Number.Length > 2)
                return BadRequest("El numero de habitacion debe ser de dos digitos");
            await locationRepository.AddLocation(location);
            return Ok();
        }
    }
    public class LocationParameterAuthenticated
    {
        public Location location { get; set; } = null!;
        public Credentials credentials { get; set; } = null!;
    }
}
