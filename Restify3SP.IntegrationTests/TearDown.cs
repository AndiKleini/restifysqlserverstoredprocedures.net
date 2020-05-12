using NUnit.Framework;
using Restify3SP.IntegrationTests.Engine;
using System;
using System.Threading.Tasks;

namespace Restify3SP.IntegrationTests
{
    [SetUpFixture]
    public class TearDown
    {
        [OneTimeTearDown]
        public async Task TearDownServiceAndDropDatabaseArtifacts()
        {
            var runner = ScriptRunnerRepository.GetInstance();
            await runner.ExecuteAllScriptsInPath(@"./DB/Cleanup");

            runner.CloseConnection();
        }
    }
}