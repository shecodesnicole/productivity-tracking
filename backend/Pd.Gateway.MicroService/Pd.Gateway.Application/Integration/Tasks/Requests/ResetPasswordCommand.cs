namespace Pd.Gateway.Application.Integration.Tasks.Requests
{
    public class ResetPasswordCommand
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
