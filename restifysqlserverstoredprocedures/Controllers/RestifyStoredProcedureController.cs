using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restify3SP;
using System;
using System.Threading.Tasks;

namespace restifysqlserverstoredprocedures.Controllers
{
    [ApiController]
    [Route("restifysp")]
    public class RestifyStoredProcedureController : ControllerBase
    {
        [HttpGet("v2/{shema}/{procedurename}/{arguments}")]
        public async Task<string> ExecuteParameterSyntax([FromRoute] string shema, [FromRoute] string procedurename, [FromRoute] string arguments)
        {
            
            var db = new DatabaseAccess("server=aag-cs.dev03.dev.admiral.local,46005;Database=Admiral_CentralSystem01;Trusted_Connection=Yes;");
            // split argument part in procedurename and parameters
            return JsonConvert.SerializeObject(
                await db.ExecuteWithParameter(shema + "." + procedurename, arguments));
        }

        [HttpGet("v2/{shema}/{procedurename}")]
        public async Task<string> ExecuteWithoutParameterSyntax([FromRoute] string shema, [FromRoute] string procedurename)
        {
            return await this.ExecuteParameterSyntax(shema, procedurename, null);
        }
    }
}
