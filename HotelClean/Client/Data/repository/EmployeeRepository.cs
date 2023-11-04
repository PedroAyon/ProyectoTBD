using Client.Data.Interfaces;
using Client.Domain.Model;
using Client.Pages;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Client.Data.repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HttpClient httpClient;
        public EmployeeRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task AddEmployee(Employee employee)
        {
            var credentials = AuthUtils.getCredentials();
            var body = new EmployeeParameterAuthenticated(employee, credentials);
            var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, Application.Json);
            using var httpResponseMessage = await httpClient.PostAsync($"api/Employees", bodyJson);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        public async Task ChangeEmployeeCredentials(int id, string newUsername, string newPassword)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteEmployee(int id)
        {
            var credentials = AuthUtils.getCredentials();
            using var httpResponseMessage = await httpClient.DeleteAsync($"api/Employees/{id}?Username={credentials.Username}&Password={credentials.Password}");
            if(!httpResponseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var credentials = AuthUtils.getCredentials();
            using var httpResponseMessage = await httpClient.GetAsync($"api/Employees/{id}?Username={credentials.Username}&Password={credentials.Password}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Employee>(responseStream);
            }
            else return null;
        }

        public async Task<IEnumerable<Employee>> GetEmployeeByName(string name)
        {
            var credentials = AuthUtils.getCredentials();
            using var httpResponseMessage = await httpClient.GetAsync($"api/Employees/?name={name}&Username={credentials.Username}&Password={credentials.Password}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<Employee>>(responseStream);
            }
            else return null;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            var credentials = AuthUtils.getCredentials();
            using var httpResponseMessage = await httpClient.GetAsync($"api/Employees/?Username={credentials.Username}&Password={credentials.Password}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<Employee>>(responseStream);
            }
            else return null;
        }

        public async Task<IEnumerable<EmployeePerformance>> GetLeastPerformingEmployees(DateOnly? startDate, DateOnly? endDate)
        {
            var credentials = AuthUtils.getCredentials();
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Employees/least_performing?startDate={Utils.dateFormatted(startDate)}&endDate={Utils.dateFormatted(endDate)}&Username={credentials.Username}&Password={credentials.Password}");
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<EmployeePerformance>>(responseStream);
            }
            else return null;
        }

        public async Task<IEnumerable<Employee>> GetServiceEmployeeList(int serviceId)
        {
            var credentials = AuthUtils.getCredentials();
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Employees/services/{serviceId}?Username={credentials.Username}&Password={credentials.Password}");
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<Employee>>(responseStream);
            }
            else return null;
        }

        public async Task<IEnumerable<EmployeePerformance>> GetTopPerformingEmployees(DateOnly? startDate, DateOnly? endDate)
        {
            var credentials = AuthUtils.getCredentials();
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Employees/top_performing?startDate={Utils.dateFormatted(startDate)}&endDate={Utils.dateFormatted(endDate)}&Username={credentials.Username}&Password={credentials.Password}");
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<EmployeePerformance>>(responseStream);
            }
            else return null;
        }

        public async Task UpdateEmployee(Employee employee)
        {
            var credentials = AuthUtils.getCredentials();
            var body = new EmployeeParameterAuthenticated(employee, credentials);
            var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, Application.Json);
            using var httpResponseMessage = await httpClient.PutAsync($"api/Employees/{employee.Id}", bodyJson);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(await httpResponseMessage.Content.ReadAsStringAsync());
        }
    }
}
