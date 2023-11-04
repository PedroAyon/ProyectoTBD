using API.Domain.Model;

namespace API.Data.Interfaces
{
    public interface IServiceRepository
    {
        Task RegisterService(Service service);
        Task UnregisterService(int serviceId);
        Task<Service> GetService(int serviceId);
        Task AssignEmployeeToService(int serviceId, int employeeId);
        Task UnregisterEmployeeFromService(int serviceId, int employeeId);
        Task StartService(int serviceId);
        Task RegisterServiceAsFinished(int serviceId);
        Task<IEnumerable<Service>> GetEmployeeServiceHistory(int employeeId, DateOnly? startDate, DateOnly? endDate);
        Task<IEnumerable<Service>> GetServiceHistory(DateOnly? startDate, DateOnly? endDate);
        Task<IEnumerable<Service>> GetServicesByStatus(string status);
        Task<IEnumerable<Service>> GetServicesByLocation(int locationId, DateOnly? startDate, DateOnly? endDate);
        Task<bool> UpdateService(Service service);
        Task<bool> ServiceExists(int id);
    }
}
