using Pd.Gateway.Application.Integration.Tasks.Models;

namespace Pd.Gateway.Application.Integration.Tasks.Requests
{
    public class RegisterUserCommand
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole UserRole { get; set; }
    }
}
