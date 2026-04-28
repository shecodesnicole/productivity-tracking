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

namespace Pd.Tasks.WebApi.Controllers
{
    [Route("api/student/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserManagementService userManagementService;
        private readonly IUnitOfWork unitOfWork;

        public UserController(IUserManagementService userManagementService, IUnitOfWork unitOfWork)
        {
            this.userManagementService = userManagementService;
            this.unitOfWork = unitOfWork;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Authorize(Roles = "Admin, Student, BasicUser, Compliance")]
        [HttpPost("GetUserProfile")]
        public async Task<IActionResult> GetUserProfileAsync([FromBody] GetUserProfileCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // --------------------------------------
                var email = User.FindFirstValue(ClaimTypes.Email);
                Console.WriteLine($"Email from token my bro: {email}");
                // --------------------------------------

                var result = await userManagementService.GetUserProfileAsync(command, cancellationToken);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureRequest<AuthenticationTokenModel>($"{e}", 500);
            }
        }

        [Authorize(Roles = "Admin, Student, BasicUser, Compliance")]
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // --------------------------------------
                var email = User.FindFirstValue(ClaimTypes.Email);
                Console.WriteLine($"Email from token my bro: {email}");
                // --------------------------------------
                var result = await userManagementService.UpdateUserAsync(command, cancellationToken);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureRequest<AuthenticationTokenModel>($"{e}", 500);
            }
        }


        
       

        [HttpPost("GetAllUsers")]
        [Authorize(Roles = "Admin, Compliance, BasicUser")]
        public async Task<IActionResult> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await userManagementService.GetAllUsersAsync(cancellationToken);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureRequest<AuthenticationTokenModel>($"{e}", 500);
            }
        }
    }
}