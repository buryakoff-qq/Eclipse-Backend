using System.Security.Cryptography.X509Certificates;

namespace Eclipse.Core.DTOs.UserDTOs
{
    public class UserRegistration
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
