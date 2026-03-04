using Pd.Tasks.Application.Features.TaskManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.TaskManagement.Persistence
{
    public interface ITaskRepository
    {
        Task<TaskModel[]> GetAllTasksAsync();
    }
}
