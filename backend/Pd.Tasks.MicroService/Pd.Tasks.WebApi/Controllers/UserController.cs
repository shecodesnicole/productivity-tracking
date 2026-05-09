using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pd.Tasks.Application.Domain.Persistence;
using Pd.Tasks.Application.Features.IAM.Models;
using Pd.Tasks.Application.Features.UsersManagement.Services;
using Pd.Tasks.Application.Features.UsersManagement.Requests;
using System.Security.Claims;

namespace Pd.Tasks.WebApi.Controllers
{
    [Authorize]
    [Route("api/tasks/[controller]")]
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

        [HttpPost("GetUserProfile")]
        public async Task<IActionResult> GetUserProfileAsync([FromBody] GetUserProfileCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await userManagementService.GetUserProfileAsync(command, cancellationToken);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureRequest<AuthenticationTokenModel>($"{e}", 500);
            }
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await userManagementService.UpdateUserAsync(command, cancellationToken);
                return FromRequestResult(result);
            }
            catch (Exception e)
            {
                return ToFailureRequest<AuthenticationTokenModel>($"{e}", 500);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("GetAllUsers")]
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
