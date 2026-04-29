using System.ComponentModel.DataAnnotations;

namespace Pd.Tasks.Application.Features.IAM.Requests
{
    public class ForgotPasswordCommand
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
