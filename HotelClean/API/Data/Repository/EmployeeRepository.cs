using API.Data.Interfaces;
using API.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HotelCleanContext _context;

        public EmployeeRepository(HotelCleanContext context)
        {
            _context = context;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL createEmployee({employee.HotelId}, {employee.Name}, {employee.LastName}, {employee.Position}, {employee.PhoneNumber}, {employee.Username}, {employee.Password})");
            return employee;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var query = await _context.Employees.FromSqlInterpolated($"CALL getEmployeeByID({id})").ToListAsync();
            if (query.Count == 0) return null!;
            return query.Single();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByHotelId(int hotelId)
        {
            return await _context.Employees.FromSqlInterpolated($"CALL getEmployeesByHotelID({hotelId})").ToListAsync();
        }

        public async Task DeleteEmployee(int id)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL deleteEmployee({id})");
        }

        public async Task ChangeEmployeeCredentials(int id, string newUsername, string newPassword)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL changeEmployeeCredentials({id}, {newUsername}, {newPassword})");
        }

        public async Task<bool> EmployeeExists(int id) => await _context.Employees.AnyAsync(e => e.Id == id);
    }
}
