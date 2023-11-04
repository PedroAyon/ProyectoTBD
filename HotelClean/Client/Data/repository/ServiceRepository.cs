using Client.Data.Interfaces;
using Client.Domain.Model;
using Client.Pages;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Client.Data.repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly HttpClient httpClient;
        public ServiceRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task AssignEmployeeToService(int serviceId, int employeeId)
        {
            var credentials = AuthUtils.getCredentials();
            var bodyJson = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, Application.Json);
            using var httpResponseMessage = await httpClient.PostAsync($"api/Services/{serviceId}/employee/{employeeId}", bodyJson);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<Service>> GetEmployeeServiceHistory(int employeeId, DateOnly? startDate, DateOnly? endDate)
        {
            var credentials = AuthUtils.getCredentials();
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Services/employee/{employeeId}?startDate={Utils.dateFormatted(startDate)}&endDate={Utils.dateFormatted(endDate)}&Username={credentials.Username}&Password={credentials.Password}");
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<Service>>(responseStream);
            }
            else return null;
        }

        public async Task<IEnumerable<Service>> GetFinishedServices()
        {
            var credentials = AuthUtils.getCredentials();
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Services/finished?Username={credentials.Username}&Password={credentials.Password}");
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<Service>>(responseStream);
            }
            else return null;
        }

        public async Task<IEnumerable<Service>> GetOngoingServices()
        {
            var credentials = AuthUtils.getCredentials();
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Services/ongoing?Username={credentials.Username}&Password={credentials.Password}");
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<Service>>(responseStream);
            }
            else return null;
        }

        public async Task<IEnumerable<Service>> GetPendingServices()
        {
            var credentials = AuthUtils.getCredentials();
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Services/pending?Username={credentials.Username}&Password={credentials.Password}");
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<Service>>(responseStream);
            }
            else return null;
        }

        public async Task<Service> GetService(int serviceId)
        {
            var credentials = AuthUtils.getCredentials();
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Services/{serviceId}?Username={credentials.Username}&Password={credentials.Password}");
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Service>(responseStream);
            }
            else return null;
        }

        public async Task<IEnumerable<Service>> GetServiceHistory(DateOnly? startDate, DateOnly? endDate)
        {
            var credentials = AuthUtils.getCredentials();
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Services/?startDate={Utils.dateFormatted(startDate)}&endDate={Utils.dateFormatted(endDate)}&Username={credentials.Username}&Password={credentials.Password}");
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<Service>>(responseStream);
            }
            else return null;
        }   

        public async Task<IEnumerable<Service>> GetServicesByLocation(int locationId, DateOnly? startDate, DateOnly? endDate)
        {
            var credentials = AuthUtils.getCredentials();
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Services/location/{locationId}?startDate={Utils.dateFormatted(startDate)}&endDate={Utils.dateFormatted(endDate)}&Username={credentials.Username}&Password={credentials.Password}");
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<Service>>(responseStream);
            }
            else return null;
        }

        public async Task RegisterService(Service service)
        {
            var credentials = AuthUtils.getCredentials();
            var body = new ServiceParameterAuthenticated(service, credentials);
            var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, Application.Json);
            using var httpResponseMessage = await httpClient.PostAsync($"api/Services/", bodyJson);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        public async Task RegisterServiceAsFinished(int serviceId)
        {
            var credentials = AuthUtils.getCredentials();
            var bodyJson = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, Application.Json);
            using var httpResponseMessage = await httpClient.PostAsync($"api/Services/{serviceId}/finish", bodyJson);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        public async Task StartService(int serviceId)
        {
            var credentials = AuthUtils.getCredentials();
            var bodyJson = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, Application.Json);
            using var httpResponseMessage = await httpClient.PostAsync($"api/Services/{serviceId}/start", bodyJson);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        public async Task UnregisterEmployeeFromService(int serviceId, int employeeId)
        {
            var credentials = AuthUtils.getCredentials();
            using var httpResponseMessage = await httpClient.DeleteAsync($"api/Services/{serviceId}/employee/{employeeId}?Username={credentials.Username}&Password={credentials.Password}");
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        public async Task UnregisterService(int serviceId)
        {
            var credentials = AuthUtils.getCredentials();
            using var httpResponseMessage = await httpClient.DeleteAsync($"api/Services/{serviceId}?Username={credentials.Username}&Password={credentials.Password}");
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(await httpResponseMessage.Content.ReadAsStringAsync());
        }


        public async Task UpdateService(Service service)
        {
            var credentials = AuthUtils.getCredentials();
            var body = new ServiceParameterAuthenticated(service, credentials);
            var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, Application.Json);            
            using var httpResponseMessage = await httpClient.PutAsync($"api/Services/{service.Id}", bodyJson);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(await httpResponseMessage.Content.ReadAsStringAsync());
        }
    }
}
