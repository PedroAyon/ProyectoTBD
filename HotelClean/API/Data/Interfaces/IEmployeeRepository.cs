using API.Domain.Model;

namespace API.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployeeById(int id);
        Task<IEnumerable<Employee>> GetEmployeesByHotelId(int hotelId);
        Task<Employee> AddEmployee(Employee employee);
        Task ChangeEmployeeCredentials(int id, string newUsername, string newPassword);
        Task DeleteEmployee(int id);
        Task<bool> EmployeeExists(int id);
    }
}
