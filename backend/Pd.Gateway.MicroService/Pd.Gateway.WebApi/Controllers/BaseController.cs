using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pd.Gateway.Application.Domain.Requests;

namespace Pd.Gateway.WebApi.Controllers
{
    [Route("api/gateway/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult FromRequestResult<TData>(RequestResult<TData> result) => StatusCode(result.StatusCode, result);

        protected IActionResult ToFailureResult<T>(string errorMessage, int statusCode = StatusCodes.Status400BadRequest)
            => StatusCode(statusCode, new RequestResult { IsSuccessful = false, ErrorMessage = errorMessage, StatusCode = statusCode });
    }
}
