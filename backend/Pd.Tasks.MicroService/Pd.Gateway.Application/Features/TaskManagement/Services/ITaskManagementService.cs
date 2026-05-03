using Pd.Tasks.Application.Domain.Requests;
using Pd.Tasks.Application.Features.TaskManagement.Models;
using Pd.Tasks.Application.Features.TaskManagement.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.TaskManagement.Services
{
    public interface ITaskManagementService
    {
        Task<RequestResult<object>> DeleteTaskAsync(DeleteTaskCommand command, CancellationToken cancellationToken);
        Task<RequestResult<TaskModel>> AddTaskAsync(AddTaskCommand taskCommand, CancellationToken cancellationToken);

        Task<RequestResult<TaskModel[]>> GetAllTasksAsync(CancellationToken cancellationToken);
        Task<RequestResult<TaskModel>> GetTaskAsync(GetTaskCommand command, CancellationToken cancellationToken);


        Task<RequestResult<TaskModel>> UpdateTaskAsync(UpdateTaskCommand command, CancellationToken cancellationToken);

        //Task<RequestResult<object>> UpdateTaskByIdAsync(CancellationToken cancellationToken);

        //Task<RequestResult<TaskModel[]>> DeleteTaskAsync(int id, CancellationToken cancellationToken);
    }
}
