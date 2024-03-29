﻿using API.Domain.Model;

namespace API.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployeeById(int id);
        Task<IEnumerable<Employee>> GetEmployeeByName(String name);
        Task<IEnumerable<Employee>> GetEmployees();
        Task<Employee> AddEmployee(Employee employee);
        Task<bool> UpdateEmployee(Employee employee);
        Task ChangeEmployeeCredentials(int id, string newUsername, string newPassword);
        Task DeleteEmployee(int id);
        Task<bool> EmployeeExists(int id);
        Task<IEnumerable<EmployeePerformance>> GetTopPerformingEmployees(DateOnly? startDate, DateOnly? endDate);
        Task<IEnumerable<EmployeePerformance>> GetLeastPerformingEmployees(DateOnly? startDate, DateOnly? endDate);
        Task<IEnumerable<Employee>> GetServiceEmployeeList(int serviceId);
        Task<Employee> GetEmployeeByCredentials(Credentials credentials);
    }
}
