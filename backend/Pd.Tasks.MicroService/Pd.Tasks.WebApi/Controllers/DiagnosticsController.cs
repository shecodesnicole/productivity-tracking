using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pd.Tasks.Application.Features.IAM.Services;

namespace Pd.Tasks.WebApi.Controllers
{
    [AllowAnonymous]
    [Route("api/tasks/[controller]")]
    [ApiController]
    public class DiagnosticsController : ControllerBase
    {
        [HttpPost("TestPasswordHash")]
        public IActionResult TestPasswordHash([FromBody] TestPasswordRequest request)
        {
            try
            {
                var isMatch = HashingService.IsHashOf(request.StoredHash, request.InputPassword);
                var newHash = HashingService.Hash(request.InputPassword);
                
                return Ok(new
                {
                    storedHash = request.StoredHash,
                    inputPassword = request.InputPassword,
                    isMatch = isMatch,
                    newHashGenerated = newHash,
                    storedHashBytes = System.Convert.FromBase64String(request.StoredHash).Length,
                    newHashBytes = System.Convert.FromBase64String(newHash).Length
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class TestPasswordRequest
    {
        public string StoredHash { get; set; }
        public string InputPassword { get; set; }
    }
}
