using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pd.Tasks.Application.Domain.Persistence;
using Pd.Tasks.Application.Features.IAM.Models;
using Microsoft.AspNetCore.Http;
using Pd.Tasks.Application.Features.TaskManagement.Models;
using Pd.Tasks.Application.Features.TaskManagement.Requests;
using Pd.Tasks.Application.Features.TaskManagement.Services;
using Pd.Tasks.Application.Features.UsersManagement.Models;
using System.Security.Claims;
using Pd.Tasks.Application.Features.UsersManagement.Services;
using Pd.Tasks.Application.Features.UsersManagement.Requests;
using Pd.Tasks.Application.Features.IAM.Services;
using Pd.Tasks.Application.Domain.Requests;
using Pd.Tasks.Application.Features.IAM.Requests;


namespace Pd.Tasks.WebApi.Controllers
{
    [Authorize]
    [Route("api/student/[controller]")]
    [ApiController]
    public class IAMController : BaseController
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IUnitOfWork unitOfWork;

        public IAMController(IAuthenticationService authenticationService, IUnitOfWork unitOfWork)
        {
            this.authenticationService = authenticationService;
            this.unitOfWork = unitOfWork;
            Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} [IAMController] CONSTRUCTOR CALLED");
            Console.Out.Flush();
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
            Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} [IAMController] Register endpoint HIT - Email: {command.Email}");
            Console.Out.Flush();

            try
            {
                Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} [IAMController] About to call RegisterUserAsync");
                Console.Out.Flush();

                var result = await authenticationService.RegisterUserAsync(command);

                Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} [IAMController] RegisterUserAsync completed, result is null: {result == null}");
                Console.Out.Flush();

                await unitOfWork.SaveChangesAsync();

                Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} [IAMController] SaveChangesAsync completed");
                Console.Out.Flush();

                // Email verification flow - return success without token
                if (result == null)
                {
                    return Ok(new RequestResult<AuthenticationTokenModel>
                    {
                        IsSuccessful = true,
                        StatusCode = 200,
                        ErrorMessage = null,
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


       
    }
}