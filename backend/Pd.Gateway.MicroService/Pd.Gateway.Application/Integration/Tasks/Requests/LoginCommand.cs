namespace Pd.Gateway.Application.Integration.Tasks.Requests
{
    public class LoginCommand
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
