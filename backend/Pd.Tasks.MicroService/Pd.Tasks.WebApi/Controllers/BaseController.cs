using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pd.Tasks.Application.Domain.Requests;

namespace Pd.Tasks.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult FromRequestResult<TData>(RequestResult<TData> requestResult) => StatusCode(requestResult.StatusCode, requestResult);

        protected IActionResult ToFailureRequest<T>(string errorMessage, int failureCode = StatusCodes.Status400BadRequest)
            => StatusCode(failureCode, new RequestResult { IsSuccessful = false, ErrorMessage = errorMessage, StatusCode = failureCode});
    }
}
