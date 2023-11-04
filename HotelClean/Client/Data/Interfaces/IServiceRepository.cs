using Client.Domain.Model;

namespace Client.Data.Interfaces
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
        Task<IEnumerable<Service>> GetPendingServices();
        Task<IEnumerable<Service>> GetOngoingServices();
        Task<IEnumerable<Service>> GetFinishedServices();
        Task<IEnumerable<Service>> GetServicesByLocation(int locationId, DateOnly? startDate, DateOnly? endDate);
    }
}
