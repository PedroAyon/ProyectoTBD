using System.Text.Json.Serialization;

namespace Client.Domain.Model
{
    public class EmployeePerformance
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("serviceCount")]
        public int ServiceCount { get; set; }
    }
}
