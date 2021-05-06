using System.ComponentModel.DataAnnotations;

namespace API.Contracts.V1.Requests
{
    public class UserLoginRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
