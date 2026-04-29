using Microsoft.Extensions.Options;
using Pd.Gateway.Application.Domain.Requests;
using Pd.Gateway.Application.Domain.Services.Http;
using Pd.Gateway.Application.Domain.Settings;
using Pd.Gateway.Application.Integration.Tasks.Models;
using Pd.Gateway.Application.Integration.Tasks.Requests;

namespace Pd.Gateway.Application.Integration.Tasks.Services
{
    public class UserIntegration : IUserIntegration
    {
        private readonly IHttpService httpService;
        private readonly IOptionsMonitor<IntegrationSettings> options;

        private string BaseUrl => $"{options.CurrentValue.TasksUrl}/tasks/User";

        public UserIntegration(IHttpService httpService, IOptionsMonitor<IntegrationSettings> options)
        {
            this.httpService = httpService;
            this.options = options;
        }

        public Task<RequestResult<UserModel>> GetUserProfileAsync(GetUserProfileCommand command)
            => httpService.PostAsync<RequestResult<UserModel>>($"{BaseUrl}/GetUserProfile", command);

        public Task<RequestResult<UserModel>> UpdateUserAsync(UpdateUserCommand command)
            => httpService.PostAsync<RequestResult<UserModel>>($"{BaseUrl}/UpdateUser", command);

        public Task<RequestResult<List<UserModel>>> GetAllUsersAsync()
            => httpService.PostAsync<RequestResult<List<UserModel>>>($"{BaseUrl}/GetAllUsers", new { });
    }
}
