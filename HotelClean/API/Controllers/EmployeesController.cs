using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data.Interfaces;
using API.Domain.Model;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IHotelRepository hotelRepository;

        public EmployeesController(IEmployeeRepository employeeRepository, IHotelRepository hotelRepository)
        {
            this.employeeRepository = employeeRepository;
            this.hotelRepository = hotelRepository;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await employeeRepository.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // GET: api/Employees/hotels/1/employees
        [HttpGet("hotels/{hotelId}/employees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesByHotel(int hotelId)
        {
            var employees = await employeeRepository.GetEmployeesByHotelId(hotelId);

            if (employees == null)
            {
                return NotFound();
            }

            return Ok(employees);
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            if (!await hotelRepository.HotelExists(employee.HotelId))
            {
                return BadRequest("El hotel no existe");
            }
            await employeeRepository.AddEmployee(employee);
            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // PUT: api/Employees/5
        [HttpPut]
        public async Task<IActionResult> ChangeEmployeeCredentials(int id, string password, string newUsername, string newPassword)
        {
            var employee = await employeeRepository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound("El empleado no existe");
            }

            if (employee.Password != password)
            {
                return BadRequest("Las contraseña es incorrecta");
            }
            await employeeRepository.ChangeEmployeeCredentials(id, newUsername, newPassword);
            return NoContent();
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await employeeRepository.DeleteEmployee(id);
            return NoContent();
        }
    }
}
