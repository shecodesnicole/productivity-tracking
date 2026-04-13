using Microsoft.EntityFrameworkCore;
using Pd.Tasks.Application.Features.TaskManagement.Models;
using Pd.Tasks.Application.Features.TaskManagement.Persistence;
using Pd.Tasks.Application.Features.TaskManagement.Requests;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;



namespace Pd.Tasks.Persistence.Accessors
{
    public class TaskRepository : ITaskRepository
    {
        private readonly LocalDbContext _context;
        public TaskRepository(LocalDbContext context) => _context = context;

        public async Task<TaskModel[]> GetAllTasksAsync()
        {
            IQueryable<TaskModel> query = _context.Tasks;
            return await query.ToArrayAsync();
        }

        public async Task<TaskModel> AddTaskAsync(TaskModel task)
        {
            await   _context.AddAsync(task);
            return task;
        }


        public async Task<TaskModel?> GetTaskByIdAsync(GetTaskCommand command)
        {
            //return await _context.Tasks.FindAsync(new object[] { id }, cancellationToken
            IQueryable<TaskModel> query = _context.Tasks.Where(t => t.Id == command.Id);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<TaskModel?> GetTaskAsync(Expression<Func<TaskModel, bool>> predicate)
        {
            IQueryable<TaskModel> query = _context.Tasks;

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<TaskModel?> UpdateTaskByIdAsync(TaskModel task, CancellationToken cancellationToken)
        {
            // Find the existing task using IQueryable
            IQueryable<TaskModel> query = _context.Tasks.Where(t => t.Id == task.Id);
            var existingTask = await query.FirstOrDefaultAsync(cancellationToken);

            if (existingTask == null)
            {
                return null; // Task not found
            }

            // Update properties
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.HoursWorked = task.HoursWorked;

            // Save changes
            await _context.SaveChangesAsync(cancellationToken);

            return existingTask;
        }

        public async Task<TaskModel> UpdateTaskAsync(TaskModel task, object taskUpdate)
        {
            _context.Entry(task).CurrentValues.SetValues(taskUpdate);

            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTaskAsync(int id, CancellationToken cancellationToken)
        {
            IQueryable<TaskModel> query = _context.Tasks.Where(t => t.Id == id);

            var task = await query.FirstOrDefaultAsync(cancellationToken);
            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

    }
}



