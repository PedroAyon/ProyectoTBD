using Client.Domain.Model;

namespace Client.Domain.Model
{
    public class LocationParameterAuthenticated
    {
        public LocationParameterAuthenticated(Location location, Credentials credentials)
        {
            this.location = location;
            this.credentials = credentials;
        }

        public Location location { get; set; } = null!;
        public Credentials credentials { get; set; } = null!;
    }
}
