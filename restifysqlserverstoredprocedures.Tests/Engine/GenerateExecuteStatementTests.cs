using NUnit.Framework;
using restifysqlserverstoredprocedures.Engine;

[TestFixture]
public class GenerateExecuteStatementTests
{
    [Test]
    [TestCase("")]
	public void FromSubroute_StoredProceduresSpecified_ExecuteSqlCreated(string spInUrl, string executeResult)
	{
        GenerateExecuteStatement.FromSubroute("");
    }
}
