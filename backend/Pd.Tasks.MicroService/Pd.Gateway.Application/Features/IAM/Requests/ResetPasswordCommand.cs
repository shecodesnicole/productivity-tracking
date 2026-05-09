using System.ComponentModel.DataAnnotations;

namespace Pd.Tasks.Application.Features.IAM.Requests
{
    public class ResetPasswordCommand
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }
    }
}
