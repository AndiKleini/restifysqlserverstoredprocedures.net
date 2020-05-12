using FluentAssertions;
using NUnit.Framework;
using Restify3SP.IntegrationTests.Abstract;
using Restify3SP.IntegrationTests.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Restify3SP.IntegrationTests
{
    [TestFixture]
    public class TestSingleResultSet : TestFixtureBase
    {
        [SetUp]
        public async Task Setup()
        {
            try 
            {
                await this.Runner.ExecuteScriptFromFile("./DB/TB_datStudents.txt");
                await this.Runner.ExecuteScriptFromFile("./DB/Fill_TB_datStudents.txt");
                await this.Runner.ExecuteScriptFromFile("./DB/SP_EmitSingleResultSet.txt");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        [Test]
        public async Task ExecuteSP_EmitsSingleResultSet_ReturnsProperJson()
        {
            var result = await this.Act("[restify].[EmitSingleResultSet]", null);

            result.Should().Be("{\"Result\":[[{\"SurName\":\"Duck\"}]],\"OutputParameter\":{},\"Return\":0}");
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.Runner.ExecuteScriptFromFile("./DB/Drop_SP_EmitSingleResultSet.txt");
            await this.Runner.ExecuteScriptFromFile("./DB/Drop_TB_datStudents.txt");
        }
    }
}
