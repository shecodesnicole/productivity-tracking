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

        [HttpGet]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await taskManagementService.GetTaskAsync(new GetTaskCommand { Id = id }, cancellationToken);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureRequest<TaskModel>($"{e.Message}", 500);
            }
        }

        [HttpPost]
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id, CancellationToken cancellationToken)
        {
            var result = await taskManagementService.DeleteTaskAsync(id, cancellationToken);
            if (!result.IsSuccessful)
                return NotFound(result.ErrorMessage);
            return NoContent();
        }
    }
}
