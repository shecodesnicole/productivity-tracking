namespace Pd.Gateway.Application.Integration.Tasks.Models
{
    public class AuthenticationTokenModel
    {
        public string Id { get; set; } = string.Empty;
        public UserModel? User { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public long ExpiresAt { get; set; }
    }
}
