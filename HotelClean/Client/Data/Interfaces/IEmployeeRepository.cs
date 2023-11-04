using Client.Domain.Model;

namespace Client.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployeeByName(String name);
        Task<IEnumerable<Employee>> GetEmployees();
        Task<IEnumerable<EmployeePerformance>> GetTopPerformingEmployees(DateOnly? startDate, DateOnly? endDate);
        Task<IEnumerable<EmployeePerformance>> GetLeastPerformingEmployees(DateOnly? startDate, DateOnly? endDate);
        Task<IEnumerable<Employee>> GetServiceEmployeeList(int serviceId);
        Task<Employee> GetEmployeeById(int id);
        Task AddEmployee(Employee employee);
        Task UpdateEmployee(Employee employee);
        Task ChangeEmployeeCredentials(int id, string newUsername, string newPassword);
        Task DeleteEmployee(int id);
    }
}
