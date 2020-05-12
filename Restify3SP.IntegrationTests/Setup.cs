using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Restify3SP.IntegrationTests.Engine;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Restify3SP.IntegrationTests
{
    [SetUpFixture]
    public class Setup
    {
        [OneTimeSetUp]
        public async Task CreateGlobalDatabaseArtifacts()
        {
            string connectionString = "Data Source=aag-cs.dev03.dev.admiral.local,46005;Initial Catalog=Admiral_CentralSystem01;Integrated Security=True";

            DataBaseAccessRepository.SetConnectionString(connectionString);
            ScriptRunnerRepository.SetInstance(new ScriptRunner(connectionString));

            var runner = ScriptRunnerRepository.GetInstance();
            await runner.ExecuteAllScriptsInPath(@"./DB/Setup");
        }
    }
}