using Newtonsoft.Json;
using Restify3SP.IntegrationTests.Engine;
using System.Threading.Tasks;

namespace Restify3SP.IntegrationTests.Abstract
{
    public class TestFixtureBase
    {
        protected ScriptRunner Runner {
            get 
            {
                return ScriptRunnerRepository.GetInstance();
            }
            private set { }
        }

        protected async Task<string> Act(string procedureName, string parameter)
        {
            return JsonConvert.SerializeObject(
                await DataBaseAccessRepository.GetInstance().ExecuteWithParameter(
                    procedureName, 
                    parameter));
        }
    }
}
