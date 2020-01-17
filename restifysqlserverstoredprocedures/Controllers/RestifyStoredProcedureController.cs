using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using restifysqlserverstoredprocedures.Engine;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace restifysqlserverstoredprocedures.Controllers
{
    [ApiController]
    [Route("restifysp")]
    public class RestifyStoredProcedureController : ControllerBase
    {
        [HttpGet("{shema}/{executeSpStatement}")]
        public async Task<string> Get([FromRoute] string shema, [FromRoute] string executeSpStatement)
        {
            Console.Write(shema);
            var db = new DatabaseAccess("server=ADM000126469;Database=restifysqlserver;Trusted_Connection=Yes;");
            return JsonConvert.SerializeObject(
                await db.Execute(GenerateExecuteStatement.FromSubroute(shema, executeSpStatement)));
        }

        [HttpGet("v2/{shema}/{procedurename}/{arguments}")]
        public async Task<string> ExecuteParameterSyntax([FromRoute] string shema, [FromRoute] string procedurename, [FromRoute] string arguments)
        {
            var db = new DatabaseAccess("server=ADM000126469;Database=restifysqlserver;Trusted_Connection=Yes;");
            // split argument part in procedurename and parameters
            return JsonConvert.SerializeObject(
                await db.ExecuteWithParameter(shema + "." + procedurename, EnrichedDynamicParameters.FromArguments(arguments)));
        }

        [HttpGet("v2/{shema}/{procedurename}")]
        public async Task<string> ExecuteWithoutParameterSyntax([FromRoute] string shema, [FromRoute] string procedurename)
        {
            return await this.ExecuteParameterSyntax(shema, procedurename, null);
        }
    }
}
