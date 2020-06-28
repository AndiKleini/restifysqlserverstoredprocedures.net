using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace restifysqlserverstoredprocedures.Controllers
{
    [ApiController]
    public class ErrorController : Controller
    {
        [Route("{dbname}/{error}")]
        public IActionResult Error()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            return Problem(
                detail: feature?.Error.StackTrace,
                title: feature?.Error.Message,
                statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}
