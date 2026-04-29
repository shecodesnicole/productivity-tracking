using Microsoft.Extensions.Options;
using Pd.Gateway.Application.Domain.Requests;
using Pd.Gateway.Application.Domain.Services.Http;
using Pd.Gateway.Application.Domain.Settings;
using Pd.Gateway.Application.Integration.Tasks.Models;
using Pd.Gateway.Application.Integration.Tasks.Requests;

namespace Pd.Gateway.Application.Integration.Tasks.Services
{
    public class IAMIntegration : IIAMIntegration
    {
        private readonly IHttpService httpService;
        private readonly IOptionsMonitor<IntegrationSettings> options;

        private string BaseUrl => $"{options.CurrentValue.TasksUrl}/tasks/IAM";

        public IAMIntegration(IHttpService httpService, IOptionsMonitor<IntegrationSettings> options)
        {
            this.httpService = httpService;
            this.options = options;
        }

        public Task<RequestResult<AuthenticationTokenModel>> LoginAsync(LoginCommand command)
            => httpService.PostAsync<RequestResult<AuthenticationTokenModel>>($"{BaseUrl}/Login", command);

        public Task<RequestResult<AuthenticationTokenModel>> RegisterUserAsync(RegisterUserCommand command)
            => httpService.PostAsync<RequestResult<AuthenticationTokenModel>>($"{BaseUrl}/Register", command);

        public Task<RequestResult<bool>> ForgotPasswordAsync(ForgotPasswordCommand command)
            => httpService.PostAsync<RequestResult<bool>>($"{BaseUrl}/ForgotPassword", command);

        public Task<RequestResult<bool>> ResetPasswordAsync(ResetPasswordCommand command)
            => httpService.PostAsync<RequestResult<bool>>($"{BaseUrl}/ResetPassword", command);
    }
}
