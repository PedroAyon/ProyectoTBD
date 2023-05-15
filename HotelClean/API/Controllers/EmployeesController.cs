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
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _employeeRepository.GetEmployees();
            if (employees == null)
            {
                return NotFound();
            }
            return Ok(employees);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);

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
            var employees = await _employeeRepository.GetEmployeesByHotelId(hotelId);

            if (employees == null)
            {
                return NotFound();
            }

            return Ok(employees);
        }


        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }


            try
            {
                await _employeeRepository.UpdateEmployee(employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _employeeRepository.EmployeeExists(id))
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

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            await _employeeRepository.AddEmployee(employee);
            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteEmployee(employee);

            return NoContent();
        }
    }
}
