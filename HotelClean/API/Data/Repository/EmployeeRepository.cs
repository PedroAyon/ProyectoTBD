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
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> GetEmployeeById(int id) => await _context.Employees.FindAsync(id);

        public async Task<IEnumerable<Employee>> GetEmployees() => await _context.Employees.ToListAsync();

        public async Task<IEnumerable<Employee>> GetEmployeesByHotelId(int hotelId) => await _context.Employees
                .Where(e => e.HotelId == hotelId)
                .ToListAsync();

        public async Task DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployee(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmployeeExists(int id) => await _context.Employees.AnyAsync(e => e.Id == id);
    }
}
