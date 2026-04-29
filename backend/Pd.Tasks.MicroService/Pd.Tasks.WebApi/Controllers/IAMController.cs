using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pd.Tasks.Application.Domain.Persistence;
using Pd.Tasks.Application.Domain.Requests;
using Pd.Tasks.Application.Features.IAM.Models;
using Pd.Tasks.Application.Features.IAM.Requests;
using Pd.Tasks.Application.Features.IAM.Services;
using Pd.Tasks.Application.Features.UsersManagement.Requests;

namespace Pd.Tasks.WebApi.Controllers
{
    [Authorize]
    [Route("api/tasks/[controller]")]
    [ApiController]
    public class IAMController : BaseController
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IUnitOfWork unitOfWork;

        public IAMController(IAuthenticationService authenticationService, IUnitOfWork unitOfWork)
        {
            this.authenticationService = authenticationService;
            this.unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpGet("Health")]
        public IActionResult Health()
        {
            return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginCommand([FromBody] LoginCommand command)
        {
            var result = await authenticationService.LoginAsync(command);
            await unitOfWork.SaveChangesAsync();
            return FromRequestResult(result);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserCommand([FromBody] RegisterUserCommand command)
        {
            try
            {
                var result = await authenticationService.RegisterUserAsync(command);
                await unitOfWork.SaveChangesAsync();

                if (result?.Data == null && result?.IsSuccessful == true)
                {
                    return Ok(new RequestResult<AuthenticationTokenModel>
                    {
                        IsSuccessful = true,
                        StatusCode = 200,
                        Data = null
                    });
                }

                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureRequest<AuthenticationTokenModel>($"{e}", 500);
            }
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            try
            {
                var result = await authenticationService.ForgotPasswordAsync(command);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureRequest<bool>($"{e}", 500);
            }
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            try
            {
                var result = await authenticationService.ResetPasswordAsync(command);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureRequest<bool>($"{e}", 500);
            }
        }
    }
}
