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

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceRepository serviceRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly ILocationRepository locationRepository;

        public ServicesController(IServiceRepository serviceRepository, IEmployeeRepository employeeRepository, ILocationRepository locationRepository)
        {
            this.serviceRepository = serviceRepository;
            this.employeeRepository = employeeRepository;
            this.locationRepository = locationRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetService(int id, [FromQuery] Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null)
                return BadRequest("No autorizado");
            var service = await serviceRepository.GetService(id);
            if (service == null)
                return NotFound();
            return Ok(service);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServiceHistory(DateOnly? startDate, DateOnly? endDate, [FromQuery] Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null || employee.Position != "Administracion")
                return BadRequest("No autorizado");
            if (startDate != null && endDate == null) endDate = DateOnly.FromDateTime(DateTime.Now);
            var services = await serviceRepository.GetServiceHistory(startDate, endDate);
            if (services == null) return NotFound();
            await AttatchLocationToServices(services);
            return Ok(services);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetEmployeeServiceHistory(int employeeId, DateOnly? startDate, DateOnly? endDate, [FromQuery] Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null || employee.Position != "Administracion")
                return BadRequest("No autorizado");
            if (!await employeeRepository.EmployeeExists(employeeId))
                return BadRequest("El empleado no existe");
            if (startDate != null && endDate == null) endDate = DateOnly.FromDateTime(DateTime.Now);
            var services = await serviceRepository.GetEmployeeServiceHistory(employeeId, startDate, endDate);
            if (services == null) return NotFound();
            await AttatchLocationToServices(services);
            return Ok(services);
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Service>>> GetPendingServices([FromQuery] Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null)
                return BadRequest("No autorizado");
            var services = await serviceRepository.GetServicesByStatus("Pendiente");
            if (services == null) return NotFound();
            await AttatchLocationToServices(services);
            return Ok(services);
        }

        [HttpGet("ongoing")]
        public async Task<ActionResult<IEnumerable<Service>>> GetOngoingServices([FromQuery] Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null)
                return BadRequest("No autorizado");
            var services = await serviceRepository.GetServicesByStatus("En Curso");
            if (services == null) return NotFound();
            await AttatchLocationToServices(services);
            return Ok(services);
        }

        [HttpGet("finished")]
        public async Task<ActionResult<IEnumerable<Service>>> GetFinishedServices([FromQuery] Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null)
                return BadRequest("No autorizado");
            var services = await serviceRepository.GetServicesByStatus("Terminado");
            if (services == null) return NotFound();
            await AttatchLocationToServices(services);
            return Ok(services);
        }

        [HttpGet("location/{locationId}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServicesByLocation(int locationId, DateOnly? startDate, DateOnly? endDate, [FromQuery] Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null)
                return BadRequest("No autorizado");
            if (!await locationRepository.LocationExists(locationId))
                return BadRequest("La locación no existe");
            if (startDate != null && endDate == null) endDate = DateOnly.FromDateTime(DateTime.Now);
            var services = await serviceRepository.GetServicesByLocation(locationId, startDate, endDate);
            if (services == null) return NotFound();
            await AttatchLocationToServices(services);
            return Ok(services);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, ServiceParameterAuthenticated serviceParameterAuthenticated)
        {
            var service = serviceParameterAuthenticated.service;
            var credentials = serviceParameterAuthenticated.credentials;
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null || employee.Position != "Administracion")
                return BadRequest("No autorizado");
            if (id != service.Id)
                return BadRequest();
            if (!await locationRepository.LocationExists(service.LocationId))
                return BadRequest("La locacion no existe");
            await serviceRepository.UpdateService(service);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Service>> PostService(ServiceParameterAuthenticated serviceParameterAuthenticated)
        {
            var service = serviceParameterAuthenticated.service;
            var credentials = serviceParameterAuthenticated.credentials;
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null || employee.Position != "Administracion")
                return BadRequest("No autorizado");
            if (!await locationRepository.LocationExists(service.LocationId))
                return BadRequest("La locacion no existe");
            await serviceRepository.RegisterService(service);
            return Ok();
        }

        [HttpPost("{serviceId}/employee/{employeeId}")]
        public async Task<IActionResult> AssignEmployeeToService(int serviceId, int employeeId, Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null || (employee.Position == "Intendencia" && employee.Id != employeeId))
                return BadRequest("No autorizado");
            if (!await employeeRepository.EmployeeExists(employeeId))
                return BadRequest("El empleado no existe");
            if (!await serviceRepository.ServiceExists(serviceId))
                return BadRequest("El servicio no existe");
            await serviceRepository.AssignEmployeeToService(serviceId, employeeId);
            return Ok();
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> StartService(int id, Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null)
                return BadRequest("No autorizado");
            var service = await serviceRepository.GetService(id);
            if (service == null || service.Status != "Pendiente") return BadRequest();
            if (!await serviceRepository.ServiceExists(id))
                return BadRequest("El servicio no existe");
            await serviceRepository.StartService(id);
            return Ok();
        }

        [HttpPost("{id}/finish")]
        public async Task<IActionResult> RegisterServiceAsFinished(int id, Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null)
                return BadRequest("No autorizado");
            var service = await serviceRepository.GetService(id);
            if (service == null || service.Status != "En Curso") return BadRequest();
            if (!await serviceRepository.ServiceExists(id))
                return BadRequest("El servicio no existe");
            await serviceRepository.RegisterServiceAsFinished(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id, [FromQuery] Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null || employee.Position != "Administracion")
                return BadRequest("No autorizado");
            await serviceRepository.UnregisterService(id);
            return NoContent();
        }

        [HttpDelete("{serviceId}/employee/{employeeId}")]
        public async Task<IActionResult> UnregisterEmployeeFromService(int serviceId, int employeeId, [FromQuery] Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null || (employee.Position == "Intendencia" && employee.Id != employeeId))
                return BadRequest("No autorizado");
            await serviceRepository.UnregisterEmployeeFromService(serviceId, employeeId);
            return NoContent();
        }

        private async Task AttatchLocationToServices(IEnumerable<Service> services)
        {
            var locations = await locationRepository.GetLocations();
            foreach (var service in  services)
                service.Location = locations.First(l => l.Id == service.LocationId);
        }

        public class ServiceParameterAuthenticated
        {
            public Service service { get; set; } = null!;
            public Credentials credentials { get; set; } = null!;
        }
    }
}
