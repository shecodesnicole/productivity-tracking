using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pd.Gateway.Application.Integration.Tasks;
using Pd.Gateway.Application.Integration.Tasks.Requests;

namespace Pd.Gateway.WebApi.Controllers
{
    [Authorize]
    [Route("api/gateway/[controller]")]
    [ApiController]
    public class TaskController : BaseController
    {
        private readonly ITasksMicroserviceIntegration integration;

        public TaskController(ITasksMicroserviceIntegration integration)
        {
            this.integration = integration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var result = await integration.Tasks.GetAllTasksAsync();
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureResult<bool>($"{e}", 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                var result = await integration.Tasks.GetTaskAsync(id);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureResult<bool>($"{e}", 500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] AddTaskCommand command)
        {
            try
            {
                var result = await integration.Tasks.AddTaskAsync(command);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureResult<bool>($"{e}", 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskCommand command)
        {
            try
            {
                var result = await integration.Tasks.UpdateTaskAsync(id, command);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureResult<bool>($"{e}", 500);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await integration.Tasks.DeleteTaskAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return ToFailureResult<bool>($"{e}", 500);
            }
        }
    }
}
