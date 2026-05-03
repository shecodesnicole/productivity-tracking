using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pd.Tasks.Application.Domain.Persistence;
using Pd.Tasks.Application.Features.TaskManagement.Models;
using Pd.Tasks.Application.Features.TaskManagement.Requests;
using Pd.Tasks.Application.Features.TaskManagement.Services;

namespace Pd.Tasks.WebApi.Controllers
{
    [Authorize]
    [Route("api/tasks/[controller]")]
    [ApiController]
    public class TaskController : BaseController
    {
        private readonly ITaskManagementService taskManagementService;
        private readonly IUnitOfWork unitOfWork;

        public TaskController(ITaskManagementService taskManagementService, IUnitOfWork unitOfWork)
        {
            this.taskManagementService = taskManagementService;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("GetAllTasks")]
        public async Task<IActionResult> GetAllTasks(CancellationToken cancellationToken)
        {
            try
            {
                var result = await taskManagementService.GetAllTasksAsync(cancellationToken);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureRequest<TaskModel>($"{e}", 500);
            }
        }

        [HttpPost("GetTask")]
        public async Task<IActionResult> GetTask([FromBody] GetTaskCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await taskManagementService.GetTaskAsync(command, cancellationToken);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureRequest<TaskModel>($"{e.Message}", 500);
            }
        }

        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask([FromBody] AddTaskCommand command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await taskManagementService.AddTaskAsync(command, cancellationToken);
                await unitOfWork.SaveChangesAsync();
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureRequest<TaskModel>($"{e}", 500);
            }
        }

        [HttpPut("UpdateTask")]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await taskManagementService.UpdateTaskAsync(command, cancellationToken);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureRequest<TaskModel>($"{e.Message}", 500);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteTask")]
        public async Task<IActionResult> DeleteTask([FromBody] DeleteTaskCommand command, CancellationToken cancellationToken)
        {
            var result = await taskManagementService.DeleteTaskAsync(command, cancellationToken);
            if (!result.IsSuccessful)
                return NotFound(result.ErrorMessage);
            return NoContent();
        }
    }
}
