using System.Text.Json.Serialization;

namespace Client.Domain.Model
{
    public class Credentials
    {
        public Credentials(string username, string password)
        {
            Username = username;
            Password = password;
        }

        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;

        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }
}
