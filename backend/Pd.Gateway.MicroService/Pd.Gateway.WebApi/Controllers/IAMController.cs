using Microsoft.AspNetCore.Mvc;
using Pd.Gateway.Application.Integration.Tasks;
using Pd.Gateway.Application.Integration.Tasks.Requests;

namespace Pd.Gateway.WebApi.Controllers
{
    [Route("api/gateway/[controller]")]
    [ApiController]
    public class IAMController : BaseController
    {
        private readonly ITasksMicroserviceIntegration integration;

        public IAMController(ITasksMicroserviceIntegration integration)
        {
            this.integration = integration;
        }

        [HttpGet("Health")]
        public IActionResult Health() => Ok(new { status = "healthy", timestamp = DateTime.UtcNow });

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var result = await integration.IAM.LoginAsync(command);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureResult<bool>($"{e}", 500);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            try
            {
                var result = await integration.IAM.RegisterUserAsync(command);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureResult<bool>($"{e}", 500);
            }
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            try
            {
                var result = await integration.IAM.ForgotPasswordAsync(command);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureResult<bool>($"{e}", 500);
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            try
            {
                var result = await integration.IAM.ResetPasswordAsync(command);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureResult<bool>($"{e}", 500);
            }
        }
    }
}
