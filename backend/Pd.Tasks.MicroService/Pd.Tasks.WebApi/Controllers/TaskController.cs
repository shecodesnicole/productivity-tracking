using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pd.Tasks.Application.Domain.Persistence;
using Pd.Tasks.Application.Features.TaskManagement.Models;
using Pd.Tasks.Application.Features.TaskManagement.Requests;
using Pd.Tasks.Application.Features.TaskManagement.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Pd.Tasks.WebApi.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask([FromBody] AddTaskCommand command, CancellationToken cancellationToken)
        {

            try
            {
                var result = await taskManagementService.AddTaskAsync(command, cancellationToken);
                return FromRequestResult(result);

            }
            catch (Exception e)
            {
                return ToFailureRequest<TaskModel>($"{e}", 500);
            }
        }

        [HttpPost("GetAllTasks")]
        public async Task<IActionResult> GetAllTasks([FromBody] CancellationToken cancellationToken)
        {

            try
            {
                var result = await taskManagementService.GetAllTasksAsync(cancellationToken);
                return FromRequestResult(result);

            }catch (Exception e)
            {
                return ToFailureRequest<TaskModel>($"{e}", 500);
            }
        }

        [HttpPost("GetTask")]
        public async Task<IActionResult> GetTaskById(GetTaskCommand command, CancellationToken cancellationToken)
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


        [HttpPost("UpdateTask")]
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

        [HttpPost("DeleteTask")]
        public async Task<IActionResult> DeleteTask(DeleteTaskCommand command, CancellationToken cancellationToken)
        {
            var result = await taskManagementService.DeleteTaskAsync(command.Id, cancellationToken);
            if (!result.IsSuccessful)
                return NotFound(result.ErrorMessage);



            return NoContent();

        }


    }
}
