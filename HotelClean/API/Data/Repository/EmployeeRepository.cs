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
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL createEmployee({employee.Name}, {employee.LastName}, {employee.Position}, {employee.PhoneNumber}, {employee.Username}, {employee.Password})");
            return employee;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var query = await _context.Employees.FromSqlInterpolated($"CALL getEmployeeByID({id})").ToListAsync();
            if (query.Count == 0) return null!;
            return query.Single();
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _context.Employees.FromSqlInterpolated($"CALL getEmployees()").ToListAsync();
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

        public async Task<IEnumerable<Employee>> GetEmployeeByName(string name)
        {
            return await _context.Employees.FromSqlInterpolated($"CALL searchEmployeeByName({name})").ToListAsync();
        }

        public async Task<IEnumerable<EmployeePerformance>> GetTopPerformingEmployees(DateOnly? startDate, DateOnly? endDate)
        {
            return await _context.Performances.FromSqlInterpolated($"CALL getTopPerformingEmployees({startDate}, {endDate})").ToListAsync();
        }

        public async Task<IEnumerable<EmployeePerformance>> GetLeastPerformingEmployees(DateOnly? startDate, DateOnly? endDate)
        {
            return await _context.Performances.FromSqlInterpolated($"CALL getLeastPerformingEmployees({startDate}, {endDate})").ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetServiceEmployeeList(int serviceId)
        {
            return await _context.Employees.FromSqlInterpolated($"CALL serviceEmployeeList({serviceId})").ToListAsync();
        }

        public async Task<bool> UpdateEmployee(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<Employee> GetEmployeeByCredentials(Credentials credentials)
        {
            var query = await _context.Employees.Where(e => e.Username == credentials.Username && e.Password == credentials.Password).ToListAsync();
            if (query.Count == 0) return null!;
            return query.Single();
        }
    }
}
