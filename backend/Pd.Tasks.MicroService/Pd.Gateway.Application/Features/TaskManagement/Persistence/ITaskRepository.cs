using Pd.Tasks.Application.Domain.Requests;
using Pd.Tasks.Application.Features.TaskManagement.Models;
using Pd.Tasks.Application.Features.TaskManagement.Requests;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Pd.Tasks.Application.Features.TaskManagement.Persistence
{
    public interface ITaskRepository
    {
        Task<bool> DeleteTaskAsync(int id, CancellationToken cancellationToken);


        Task<TaskModel[]> GetAllTasksAsync();
        Task<TaskModel?> GetTaskByIdAsync(GetTaskCommand command);
        Task<TaskModel?> GetTaskAsync(Expression<Func<TaskModel, bool>> predicate);
        //Task<TaskModel> UpdateTaskByIdAsync(UpdateTaskCommand command, object taskUpdate);
        Task<TaskModel> UpdateTaskAsync(TaskModel task, object taskUpdate);

        Task<TaskModel> AddTaskAsync(TaskModel task);
        Task SaveAsync();
    }
}
