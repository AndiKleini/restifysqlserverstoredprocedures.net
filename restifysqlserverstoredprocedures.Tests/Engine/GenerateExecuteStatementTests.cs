using NUnit.Framework;
using restifysqlserverstoredprocedures.Engine;

[TestFixture]
public class GenerateExecuteStatementTests
{
    [Test]
    [TestCase("shema", "/sendMeRoses()", "exec shema.sendMeRoses")]
    [TestCase("shema", "/sendMeRoses(language 'en')", "exec shema.sendMeRoses @language = 'en'")]
    [TestCase("shema", "/sendMeRoses(customerid 12345)", "exec shema.sendMeRoses @customerid = 12345")]
    [TestCase("shema", "/sendMeRoses(customerid 12345, language 'en')", "exec shema.sendMeRoses @customerid = 12345, @language = 'en'")]
    [TestCase("shema", "/sendMeRoses(customerid 12345, language 'en', mandatorid 123456)", "exec shema.sendMeRoses @customerid = 12345, @language = 'en', @mandatorid = 123456")]
    [TestCase("shema", "/sendMeRoses(customerid 12345, language 'en', mandatorid 123456, includeall 1)", "exec shema.sendMeRoses @customerid = 12345, @language = 'en', @mandatorid = 123456, @includeall = 1")]
    [TestCase(
            "bonus",
            "/bonus/GetBonusCardByID(BonusCardId AC0AB7D8-82B8-E611-80D5-000C2988A42C, Language 'de')",
            "exec bonus.GetBonusCardByID @BonusCardId = AC0AB7D8-82B8-E611-80D5-000C2988A42C, @Language = 'de'")]
	public void FromSubroute_StoredProceduresSpecified_ExecuteSqlCreated(string shema, string spInUrl, string executeResult)
	{
        string yieldExecuteSql = GenerateExecuteStatement.FromSubroute(shema, spInUrl);
        
        StringAssert.AreEqualIgnoringCase(executeResult, yieldExecuteSql);
    }
}
