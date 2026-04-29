using Pd.Tasks.Application.Domain.Requests;
using Pd.Tasks.Application.Features.TaskManagement.Models;
using Pd.Tasks.Application.Features.TaskManagement.Persistence;
using Pd.Tasks.Application.Features.TaskManagement.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.TaskManagement.Services //functions to call the API 
{
    public class TaskManagementService : ITaskManagementService
    {
        private readonly ITaskRepository _taskRepository;
        public TaskManagementService(ITaskRepository taskRepository) => _taskRepository = taskRepository;

        public async Task<RequestResult<TaskModel[]>> GetAllTasksAsync(CancellationToken cancellationToken)
        {
            var result = await _taskRepository.GetAllTasksAsync();
            return new RequestResult<TaskModel[]>//gives a list of tasks models the return type
            {
                IsSuccessful = true,
                StatusCode = 200,
                Data = result
            };
        }

        public async Task<RequestResult<TaskModel>> AddTaskAsync(AddTaskCommand taskCommand, CancellationToken cancellationToken)
        {
            // Convert DueDate to UTC if provided, to fix PostgreSQL timestamp with time zone issue
            DateTime? dueDateUtc = null;
            if (taskCommand.DueDate.HasValue)
            {
                dueDateUtc = taskCommand.DueDate.Value.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(taskCommand.DueDate.Value, DateTimeKind.Utc)
                    : taskCommand.DueDate.Value;
            }

            var taskInfo  = new TaskModel
            {
                Title = taskCommand.Title,
                Description = taskCommand.Description,
                Status = taskCommand.Status,
                HoursWorked = taskCommand.HoursWorked,
                CreatedAt = DateTime.UtcNow,
                DueDate = dueDateUtc,
                IsActive = true,
                CompletedAt = null // initially not completed
            };

            var result = await _taskRepository.AddTaskAsync(taskInfo);

            return new RequestResult<TaskModel>
            {
                IsSuccessful = true,
                StatusCode = 200,
                Data = taskInfo
            };
        }
        public async Task<RequestResult<TaskModel>> GetTaskAsync(GetTaskCommand command, CancellationToken cancellationToken)
        {

            var task = await _taskRepository.GetTaskAsync(t => t.Id == command.Id);

            if (task == null)
                return new RequestResult<TaskModel>
                {
                    IsSuccessful = false,
                    StatusCode = 404,
                    ErrorMessage = $"Task with ID {command.Id} not found.",
                    Data = null
                };


            return new RequestResult<TaskModel>
            {
                IsSuccessful = true,
                StatusCode = 200,
                Data = task
            };
        }

        public async Task<RequestResult<TaskModel>> UpdateTaskAsync(UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetTaskAsync(u => u.Id == command.Id);

            if (task == null)
                return new RequestResult<TaskModel>
                {
                    IsSuccessful = false,
                    StatusCode = 404,
                    ErrorMessage = "Task not found",
                    Data = null
                };

            if (command.Title != null) task.Title = command.Title;
            if (command.Description != null) task.Description = command.Description;
            if (command.Status.HasValue) task.Status = command.Status.Value;
            if (command.DueDate.HasValue)
            {
                // Convert DueDate to UTC if unspecified, to fix PostgreSQL timestamp with time zone issue
                task.DueDate = command.DueDate.Value.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(command.DueDate.Value, DateTimeKind.Utc)
                    : command.DueDate.Value;
            }

            await _taskRepository.SaveAsync();

            return new RequestResult<TaskModel>
            {
                IsSuccessful = true,
                StatusCode = 200,
                Data = task
            };
        }

        public async Task<RequestResult<object>> DeleteTaskAsync(int id, CancellationToken cancellationToken)
        {
            var deleted = await _taskRepository.DeleteTaskAsync(id, cancellationToken);

            if (!deleted)
            {
                return new RequestResult<object>
                {
                    IsSuccessful = false,
                    StatusCode = 404,
                    ErrorMessage = $"Task with ID {id} not found."
                };
            }

            return new RequestResult<object>
            {
                IsSuccessful = true,
                StatusCode = 204, // No Content
                Data = null
            };
        }

    }


}
