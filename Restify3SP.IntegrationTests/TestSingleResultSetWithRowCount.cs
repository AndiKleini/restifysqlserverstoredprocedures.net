using FluentAssertions;
using NUnit.Framework;
using Restify3SP.IntegrationTests.Abstract;
using System;
using System.Diagnostics;

using System.Threading.Tasks;

namespace Restify3SP.IntegrationTests
{
    [TestFixture]
    public class TestSingleResultSetWithRowCount : TestFixtureBase
    {
        [SetUp]
        public async Task Setup()
        {
            try
            {
                await this.Runner.ExecuteScriptFromFile("./DB/TB_datStudents.txt");
                await this.Runner.ExecuteScriptFromFile("./DB/Fill_TB_datStudents.txt");
                await this.Runner.ExecuteScriptFromFile("./DB/SP_EmitSingleResultSetWithRowCount.txt");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        [Test]
        public async Task ExecuteSP_EmitsSingleResultSet_ReturnsProperJson()
        {
            var result = await this.Act("[restify].[EmitSingleResultSetWithRowCount]", null);

            result.Should().Be("{\"Result\":[[{\"SurName\":\"Duck\"}],[{\"Number of selected rows\":1}]],\"OutputParameter\":{},\"Return\":0}");
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.Runner.ExecuteScriptFromFile("./DB/Drop_SP_EmitSingleResultSetWithRowCount.txt");
            await this.Runner.ExecuteScriptFromFile("./DB/Drop_TB_datStudents.txt");
        }
    }
}
