using HiveFS.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HiveFS.TakeHomeAssessment.Controllers
{
    /// <summary>
    /// Controllers that inherit this will require authorization
    /// </summary>
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class HiveFsBaseController : ControllerBase
    {
        protected IActionResult MapResult<T>(ApiResult<T> result)
        {
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return result.HttpStatusCode switch
            {
                HttpStatusCode.NotFound => StatusCode(StatusCodes.Status404NotFound, "Data was not found."),
                HttpStatusCode.BadRequest => BadRequest(),
                HttpStatusCode.Unauthorized => Unauthorized("Not Authenticated."),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Internal Error Occurred."),
            };
        }
    }
}
