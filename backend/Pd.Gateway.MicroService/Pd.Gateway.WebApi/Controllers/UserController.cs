using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pd.Gateway.Application.Integration.Tasks;
using Pd.Gateway.Application.Integration.Tasks.Requests;

namespace Pd.Gateway.WebApi.Controllers
{
    [Authorize]
    [Route("api/gateway/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly ITasksMicroserviceIntegration integration;

        public UserController(ITasksMicroserviceIntegration integration)
        {
            this.integration = integration;
        }

        [HttpPost("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile([FromBody] GetUserProfileCommand command)
        {
            try
            {
                var result = await integration.Users.GetUserProfileAsync(command);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureResult<bool>($"{e}", 500);
            }
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            try
            {
                var result = await integration.Users.UpdateUserAsync(command);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureResult<bool>($"{e}", 500);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await integration.Users.GetAllUsersAsync();
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureResult<bool>($"{e}", 500);
            }
        }
    }
}
