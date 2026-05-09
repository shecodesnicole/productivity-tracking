using Pd.Tasks.Application.Domain.Requests;
using Pd.Tasks.Application.Features.IAM.Models;
using Pd.Tasks.Application.Features.IAM.Requests;
using Pd.Tasks.Application.Features.UsersManagement.Requests;

namespace Pd.Tasks.Application.Features.IAM.Services
{
    public interface IAuthenticationService
    {
        Task<RequestResult<AuthenticationTokenModel>> LoginAsync(LoginCommand request);
        Task<RequestResult<AuthenticationTokenModel>> RegisterUserAsync(RegisterUserCommand request);
        Task<RequestResult<bool>> ForgotPasswordAsync(ForgotPasswordCommand command);
        Task<RequestResult<bool>> ResetPasswordAsync(ResetPasswordCommand command);
    }
}