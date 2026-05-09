using Pd.Tasks.Application.Domain.Enums;

namespace Pd.Tasks.Application.Features.UsersManagement.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole UserRole { get; set; }
        public bool IsActive { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
    }
}
