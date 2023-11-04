using Client.Domain.Model;

namespace Client.Data
{
    public class AuthUtils
    {
        public static bool IsAuthenticated()
        {
            return true;
        }

        public static Credentials getCredentials()
        {
            return new Credentials("admin", "123");
        }
    }
}
