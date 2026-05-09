using Pd.Gateway.Application.Domain.Requests;
using Pd.Gateway.Application.Integration.Tasks.Models;
using Pd.Gateway.Application.Integration.Tasks.Requests;

namespace Pd.Gateway.Application.Integration.Tasks.Services
{
    public interface IIAMIntegration
    {
        Task<RequestResult<AuthenticationTokenModel>> LoginAsync(LoginCommand command);
        Task<RequestResult<AuthenticationTokenModel>> RegisterUserAsync(RegisterUserCommand command);
        Task<RequestResult<bool>> ForgotPasswordAsync(ForgotPasswordCommand command);
        Task<RequestResult<bool>> ResetPasswordAsync(ResetPasswordCommand command);
    }
}
