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

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id, [FromQuery] Credentials credentials)
        {
            if (await employeeRepository.GetEmployeeByCredentials(credentials) == null)
                return BadRequest("No autorizado");
            var employee = await employeeRepository.GetEmployeeById(id);
            if (employee == null)
                return NotFound();
            
            return Ok(employee);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(string? name, [FromQuery] Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null || employee.Position != "Administracion")
                return BadRequest("No autorizado");
            IEnumerable<Employee> employees;
            if (name == null)
                employees = await employeeRepository.GetEmployees();
            else
                employees = await employeeRepository.GetEmployeeByName(name);

            if (employees == null)
                return NotFound();

            return Ok(employees);
        }

        [HttpGet("top_performing")]
        public async Task<ActionResult<IEnumerable<EmployeePerformance>>> GetTopPerformingEmployees(DateOnly? startDate, DateOnly? endDate, [FromQuery] Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null || employee.Position != "Administracion")
                return BadRequest("No autorizado");
            if (startDate != null && endDate == null) endDate = DateOnly.FromDateTime(DateTime.Now);
            var employees = await employeeRepository.GetTopPerformingEmployees(startDate, endDate);
            if (employees == null) return NotFound();
            return Ok(employees);
        }

        [HttpGet("least_performing")]
        public async Task<ActionResult<IEnumerable<EmployeePerformance>>> GetLeastPerformingEmployees(DateOnly? startDate, DateOnly? endDate, [FromQuery] Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null || employee.Position != "Administracion")
                return BadRequest("No autorizado");
            if (startDate != null && endDate == null) endDate = DateOnly.FromDateTime(DateTime.Now);
            var employees = await employeeRepository.GetTopPerformingEmployees(startDate, endDate);
            if (employees == null) return NotFound();
            return Ok(employees);
        }

        [HttpGet("services/{serviceId}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetServiceEmployeeList(int serviceId, [FromQuery] Credentials credentials)
        {
            var employee = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employee == null || employee.Position != "Administracion")
                return BadRequest("No autorizado");
            var employees = await employeeRepository.GetServiceEmployeeList(serviceId);
            if (employees == null) return NotFound();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeParameterAuthenticated employeeParameterAuthenticated)
        {
            var credentials = employeeParameterAuthenticated.credentials;
            var employeeAuth = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employeeAuth == null || employeeAuth.Position != "Administracion")
                return BadRequest("No autorizado");
            string? errorMessage = null;
            var employee = employeeParameterAuthenticated.employee;
            if (employee.PhoneNumber != null && employee.PhoneNumber.Length != 10)
                errorMessage = "Numero de telefono no es de 10 digitos";
            if (employee.Position == null || !Employee.positions.Contains(employee.Position))
                errorMessage = "No existe la posicion del empleado";
            if (employee.Username == null || employee.Username.Length > 20)
                errorMessage = "Nombre de usuario nulo o demasiado largo";
            if (employee.Password == null || employee.Password.Length > 20)
                errorMessage = "Password nulo o demasiado largo";
            if (employee.Name == null || employee.Name.Length > 50) 
                errorMessage = "Nombre nulo o demasiado largo";
            if (employee.LastName == null || employee.LastName.Length > 50)
                errorMessage = "Apellido nulo o demasiado largo";
            if (errorMessage != null)
                return BadRequest(errorMessage);
            await employeeRepository.AddEmployee(employee);
            return Ok();
        }

        [HttpPut("credentials")]
        public async Task<IActionResult> ChangeEmployeeCredentials(string newUsername, string newPassword, Credentials credentials)
        {
            var employeeAuth = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employeeAuth == null || employeeAuth.Position != "Administracion")
                return BadRequest("No autorizado");
            string? errorMessage = null;
            if (newUsername.Length > 20)
                return BadRequest("Nombre de usuario demasiado largo");
            if (newPassword.Length > 20)
                return BadRequest("Password de usuario demasiado largo");
            await employeeRepository.ChangeEmployeeCredentials(employeeAuth.Id, newUsername, newPassword);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeParameterAuthenticated employeeParameterAuthenticated)
        {
            var credentials = employeeParameterAuthenticated.credentials;
            var employee = employeeParameterAuthenticated.employee;
            var employeeAuth = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employeeAuth == null || employeeAuth.Position != "Administracion")
                return BadRequest("No autorizado");
            if (id != employee.Id)
                return BadRequest();
            await employeeRepository.UpdateEmployee(employee);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id, [FromQuery] Credentials credentials)
        {
            var employeeAuth = await employeeRepository.GetEmployeeByCredentials(credentials);
            if (employeeAuth == null || employeeAuth.Position != "Administracion" || employeeAuth.Id == id)
                return BadRequest("No autorizado");
            await employeeRepository.DeleteEmployee(id);
            return NoContent();
        }
    }

    public class EmployeeParameterAuthenticated
    {
        public Employee employee { get; set; } = null!;
        public Credentials credentials { get; set; } = null!;
    }
}
