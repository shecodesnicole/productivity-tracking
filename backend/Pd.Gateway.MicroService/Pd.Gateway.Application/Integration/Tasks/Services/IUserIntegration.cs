using Pd.Gateway.Application.Domain.Requests;
using Pd.Gateway.Application.Integration.Tasks.Models;
using Pd.Gateway.Application.Integration.Tasks.Requests;

namespace Pd.Gateway.Application.Integration.Tasks.Services
{
    public interface IUserIntegration
    {
        Task<RequestResult<UserModel>> GetUserProfileAsync(GetUserProfileCommand command);
        Task<RequestResult<UserModel>> UpdateUserAsync(UpdateUserCommand command);
        Task<RequestResult<List<UserModel>>> GetAllUsersAsync();
    }
}
