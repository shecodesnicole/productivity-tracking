using Microsoft.EntityFrameworkCore;
using Pd.Tasks.Application.Features.TaskManagement.Models;
using Pd.Tasks.Application.Features.TaskManagement.Persistence;
using System;
using System.Collections.Generic;
using System.Text;



namespace Pd.Tasks.Persistence.Accessors
{
    public class TaskRepository: ITaskRepository 
    {
        private readonly LocalDbContext localDbContext;


        public async Task<TaskModel[]> GetAllTasksAsync()
        {
            IQueryable<TaskModel> query = localDbContext.Tasks;
            return await query.ToArrayAsync();

        }
    }
}
