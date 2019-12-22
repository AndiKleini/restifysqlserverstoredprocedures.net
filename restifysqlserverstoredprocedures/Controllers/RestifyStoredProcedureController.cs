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
    }
}
