using API.Domain.Model;

namespace API.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployees();
        Task<Employee> GetEmployeeById(int id);
        Task<IEnumerable<Employee>> GetEmployeesByHotelId(int hotelId);
        Task<Employee> AddEmployee(Employee employee);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(Employee employee);
        Task<bool> EmployeeExists(int id);
    }
}
