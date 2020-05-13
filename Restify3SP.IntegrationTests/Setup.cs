using NUnit.Framework;
using Restify3SP.IntegrationTests.Engine;
using System.Configuration;
using System.Threading.Tasks;

namespace Restify3SP.IntegrationTests
{
    [SetUpFixture]
    public class Setup
    {
        [OneTimeSetUp]
        public async Task CreateGlobalDatabaseArtifacts()
        {
            var configuration = ConfigurationManager.OpenExeConfiguration("./Restify3SP.IntegrationTests.dll");
            string connectionString = configuration.AppSettings.Settings["SQLServer"].Value;

            DataBaseAccessRepository.SetConnectionString(connectionString);
            ScriptRunnerRepository.SetInstance(new ScriptRunner(connectionString));

            var runner = ScriptRunnerRepository.GetInstance();
            await runner.ExecuteAllScriptsInPath(@"./DB/Setup");
        }
    }
}