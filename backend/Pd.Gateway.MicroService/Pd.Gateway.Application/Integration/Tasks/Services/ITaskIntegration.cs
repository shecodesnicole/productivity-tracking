using Pd.Gateway.Application.Domain.Requests;
using Pd.Gateway.Application.Integration.Tasks.Models;
using Pd.Gateway.Application.Integration.Tasks.Requests;

namespace Pd.Gateway.Application.Integration.Tasks.Services
{
    public interface ITaskIntegration
    {
        Task<RequestResult<List<TaskModel>>> GetAllTasksAsync();
        Task<RequestResult<TaskModel>> GetTaskAsync(int id);
        Task<RequestResult<TaskModel>> AddTaskAsync(AddTaskCommand command);
        Task<RequestResult<TaskModel>> UpdateTaskAsync(int id, UpdateTaskCommand command);
        Task DeleteTaskAsync(int id);
    }
}
