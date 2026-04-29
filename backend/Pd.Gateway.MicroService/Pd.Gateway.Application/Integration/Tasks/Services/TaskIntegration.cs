using Microsoft.Extensions.Options;
using Pd.Gateway.Application.Domain.Requests;
using Pd.Gateway.Application.Domain.Services.Http;
using Pd.Gateway.Application.Domain.Settings;
using Pd.Gateway.Application.Integration.Tasks.Models;
using Pd.Gateway.Application.Integration.Tasks.Requests;

namespace Pd.Gateway.Application.Integration.Tasks.Services
{
    public class TaskIntegration : ITaskIntegration
    {
        private readonly IHttpService httpService;
        private readonly IOptionsMonitor<IntegrationSettings> options;

        private string BaseUrl => $"{options.CurrentValue.TasksUrl}/tasks/Task";

        public TaskIntegration(IHttpService httpService, IOptionsMonitor<IntegrationSettings> options)
        {
            this.httpService = httpService;
            this.options = options;
        }

        public Task<RequestResult<List<TaskModel>>> GetAllTasksAsync()
            => httpService.GetAsync<RequestResult<List<TaskModel>>>(BaseUrl);

        public Task<RequestResult<TaskModel>> GetTaskAsync(int id)
            => httpService.GetAsync<RequestResult<TaskModel>>($"{BaseUrl}/{id}");

        public Task<RequestResult<TaskModel>> AddTaskAsync(AddTaskCommand command)
            => httpService.PostAsync<RequestResult<TaskModel>>(BaseUrl, command);

        public Task<RequestResult<TaskModel>> UpdateTaskAsync(int id, UpdateTaskCommand command)
        {
            command.Id = id;
            return httpService.PutAsync<RequestResult<TaskModel>>($"{BaseUrl}/{id}", command);
        }

        public Task DeleteTaskAsync(int id)
            => httpService.DeleteAsync<object>($"{BaseUrl}/{id}");
    }
}
