namespace Client.Domain.Model
{
    public class ServiceParameterAuthenticated
    {
        public ServiceParameterAuthenticated(Service service, Credentials credentials)
        {
            this.service = service;
            this.credentials = credentials;
        }

        public Service service { get; set; } = null!;
        public Credentials credentials { get; set; } = null!;
    }
}
