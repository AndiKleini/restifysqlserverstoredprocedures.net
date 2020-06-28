using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restify3SP;

namespace restifysqlserverstoredprocedures.Controllers
{
    [ApiController]
    public class CallSpController : Controller
    {
        private DatabaseAccess dataBaseAccess;
        public CallSpController(DatabaseAccess access)
        {
            this.dataBaseAccess = access;
        }
        [HttpGet("{dbname}/{schema}/{procedurename}/{arguments}")]
        public async Task<string> ExecuteParameterSyntax([FromRoute] string schema, [FromRoute] string procedurename, [FromRoute] string arguments)
        {
            return JsonConvert.SerializeObject(
                await this.dataBaseAccess.ExecuteWithParameter(schema + "." + procedurename, arguments));
        }
        [HttpGet("{dbname}/{schema}/{procedurename}")]
        public async Task<string> ExecuteParameterSyntax([FromRoute] string schema, [FromRoute] string procedurename)
        {
            return await this.ExecuteParameterSyntax(schema, procedurename, null);
        }
    }
}