using Client.Data.Interfaces;
using Client.Domain.Model;
using Client.Pages;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using System.Text;
using System.Xml.Linq;

namespace Client.Data.repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly HttpClient httpClient;

        public LocationRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task AddLocation(Location location)
        {
            var credentials = AuthUtils.getCredentials();
            var body = new LocationParameterAuthenticated(location, credentials);
            var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, Application.Json);
            using var httpResponseMessage = await httpClient.PostAsync($"api/Locations", bodyJson);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<Location>> GetLocations()
        {
            var credentials = AuthUtils.getCredentials();
            using var httpResponseMessage = await httpClient.GetAsync($"api/Locations/?Username={credentials.Username}&Password={credentials.Password}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<Location>>(responseStream);
            }
            else return null;
        }

        public async Task UpdateLocation(Location location)
        {
            var credentials = AuthUtils.getCredentials();
            var body = new LocationParameterAuthenticated(location, credentials);
            var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, Application.Json);
            using var httpResponseMessage = await httpClient.PutAsync($"api/Employees/{location.Id}", bodyJson);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(await httpResponseMessage.Content.ReadAsStringAsync());
        }
    }
}
