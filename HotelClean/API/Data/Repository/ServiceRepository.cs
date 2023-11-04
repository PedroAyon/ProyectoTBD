using API.Data.Interfaces;
using API.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly HotelCleanContext _context;

        public ServiceRepository(HotelCleanContext context)
        {
            _context = context;
        }

        public async Task AssignEmployeeToService(int serviceId, int employeeId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL assignEmployeeToService({serviceId}, {employeeId})");
        }

        public async Task<IEnumerable<Service>> GetEmployeeServiceHistory(int employeeId, DateOnly? startDate, DateOnly? endDate)
        {
            return await _context.Services.FromSqlInterpolated($"CALL getEmployeeServiceHistory({employeeId}, {startDate}, {endDate})").ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetServicesByStatus(string status)
        {
            return await _context.Services.FromSqlInterpolated($"CALL getServicesByStatus({status})").ToListAsync();
        }

        public async Task<Service> GetService(int serviceId)
        {
            var query = await _context.Services.FromSqlInterpolated($"CALL getService({serviceId})").ToListAsync();
            if (query.Count == 0) return null!;
            return query.Single();
        }

        public async Task<IEnumerable<Service>> GetServiceHistory(DateOnly? startDate, DateOnly? endDate)
        {
            return await _context.Services.FromSqlInterpolated($"CALL getServiceHistory({startDate}, {endDate})").ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetServicesByLocation(int locationId, DateOnly? startDate, DateOnly? endDate)
        {
            return await _context.Services.FromSqlInterpolated($"CALL getServicesByLocation({locationId}, {startDate}, {endDate})").ToListAsync();
        }

        public async Task RegisterService(Service service)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL registerService({service.LocationId}, {service.Type})");
        }

        public async Task RegisterServiceAsFinished(int serviceId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL registerServiceAsFinished({serviceId})");
        }

        public async Task<bool> ServiceExists(int id) => await _context.Services.AnyAsync(s => s.Id == id);

        public async Task StartService(int serviceId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL startService({serviceId})");
        }

        public async Task UnregisterEmployeeFromService(int serviceId, int employeeId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL unregisterEmployeeFromService({serviceId}, {employeeId})");
        }

        public async Task UnregisterService(int serviceId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL unregisterService({serviceId})");
        }

        public async Task<bool> UpdateService(Service service)
        {
            _context.Entry(service).State = EntityState.Modified;
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
    }
}
