using Client.Domain.Model;

namespace Client.Domain.Model
{
    public class EmployeeParameterAuthenticated
    {
        public EmployeeParameterAuthenticated(Employee employee, Credentials credentials)
        {
            this.employee = employee;
            this.credentials = credentials;
        }

        public Employee employee { get; set; } = null!;
        public Credentials credentials { get; set; } = null!;
    }
}
