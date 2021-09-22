using FluentAssertions;
using NUnit.Framework;
using Restify3DS.Tests.Help;
using Restify3SP;
using System.Collections.Generic;
using System.Data;

namespace Restify3DS.Tests
{
    [TestFixture]
    public class EnrichedDynamicParametersTests
    {
        [Test]
        [TestCaseSource("argumentStrings")]
        public void FromArguments_ValidArgumentsString_ParameterCreated(string arguments, ParameterInfo[] inputParameterInfos, ParameterInfo[] outputParameterInfos)
        {
            var yieldParameters = EnrichedDynamicParameters.FromArguments(arguments);

            yieldParameters.Should().ContainParameters(inputParameterInfos, outputParameterInfos);
        }

        [Test]
        public void FromArguments_NullAsArgumentsString_ParameterCreated()
        {
            var yieldParameters = EnrichedDynamicParameters.FromArguments(null);

            yieldParameters.Should().ContainParameters(new ParameterInfo[] { }, new ParameterInfo[] { });
        }


        [Test]
        public void GetParameterForDirection_DirectionInput_ReturnsCollectionOfInputParameter()
        {
            string testName = "test";
            object testValue = new object();
            var parameterInfo = new KeyValuePair<string, object>(testName, testValue);
            var instanceUnderTest = new EnrichedDynamicParameters();
            instanceUnderTest.AddParameter(
                testName, 
                testValue, 
                null, 
                ParameterDirection.Input, 
                null);

            var yieldParameter = instanceUnderTest.GetParameterForDirection(ParameterDirection.Input);

            yieldParameter.Should().Contain(parameterInfo);
        }

        [Test]
        public void GetParameterForDirection_DirectionOutput_ReturnsCollectionOfOutputParameter()
        {
            string testName = "test";
            object testValue = new object();
            var parameterInfo = new KeyValuePair<string, object>(testName, testValue);
            var instanceUnderTest = new EnrichedDynamicParameters();
            instanceUnderTest.AddParameter(
                testName,
                testValue,
                null,
                ParameterDirection.Output,
                null);

            var yieldParameter = instanceUnderTest.GetParameterForDirection(ParameterDirection.Output);

            yieldParameter.Should().Contain(parameterInfo);
        }

        public static IEnumerable<TestCaseData> argumentStrings
        {
            get
            {
                yield return new TestCaseData(
                    "@language = 'en'", 
                    new ParameterInfo[] 
                    { 
                        ParameterInfo.From("language", "'en'") 
                    },
                    new ParameterInfo[] { });

                yield return new TestCaseData(
                   "@number = 27",
                   new ParameterInfo[]
                   {
                       ParameterInfo.From("number", "27")
                   },
                   new ParameterInfo[] { });

                yield return new TestCaseData(
                    "@language = 'en', @number = 12",
                    new ParameterInfo[]
                    {
                        ParameterInfo.From("language", "'en'"),
                        ParameterInfo.From("number", "12")
                    },
                    new ParameterInfo[] { });

                yield return new TestCaseData(
                  "@language = 'en', @number = 13",
                  new ParameterInfo[]
                  {
                        ParameterInfo.From("language", "'en'"),
                        ParameterInfo.From("number", "13")
                  },
                  new ParameterInfo[] { });

                yield return new TestCaseData(
                    "@number = 13 out",
                    new ParameterInfo[] { },
                    new ParameterInfo[]
                    {
                          ParameterInfo.From("number", "13", ParameterDirection.Output)
                    });

                yield return new TestCaseData(
                   "@number = 13 out, @language = 'en' out",
                   new ParameterInfo[] { },
                   new ParameterInfo[]
                   {
                          ParameterInfo.From("number", "13", ParameterDirection.Output),
                          ParameterInfo.From("language", "'en'", ParameterDirection.Output)
                   });

                yield return new TestCaseData(
                  "@number = 13 out, @size = 23, @language = 'en' out",
                  new ParameterInfo[] 
                  {
                     ParameterInfo.From("size", "23", ParameterDirection.Input)
                  },
                  new ParameterInfo[]
                  {
                          ParameterInfo.From("number", "13", ParameterDirection.Output),
                          ParameterInfo.From("language", "'en'", ParameterDirection.Output)
                  });

                yield return new TestCaseData(
                    "@id = 'cd82ff5f-23c6-487c-b615-55d16737f8a4'",
                    new ParameterInfo[]
                    {
                         ParameterInfo.From("id", "'cd82ff5f-23c6-487c-b615-55d16737f8a4'", ParameterDirection.Input)
                    },
                    new ParameterInfo[] { });

                yield return new TestCaseData(
                   "@id = 'cd82ff5f-23c6-487c-b615-55d16737f8a4', @idOut = '8552ab5e-422d-407d-9174-6986f65f7b89' out",
                   new ParameterInfo[]
                   {
                         ParameterInfo.From("id", "'cd82ff5f-23c6-487c-b615-55d16737f8a4'", ParameterDirection.Input)
                   },
                   new ParameterInfo[] 
                   {
                        ParameterInfo.From("idOut", "'8552ab5e-422d-407d-9174-6986f65f7b89'", ParameterDirection.Output)
                   });

                yield return new TestCaseData(
                  "@MandatorNumber=9, @SearchParameter=tosca_payment@admiral.at",
                  new ParameterInfo[]
                  {
                         ParameterInfo.From("MandatorNumber", "9", ParameterDirection.Input),
                         ParameterInfo.From("SearchParameter", "tosca_payment@admiral.at", ParameterDirection.Input)
                  },
                  new ParameterInfo[] { });

                yield return new TestCaseData(
                 "@MandatorNumber=9, @Machinenumber=<<!?TER-04523>>",
                 new ParameterInfo[]
                 {
                         ParameterInfo.From("MandatorNumber", "9", ParameterDirection.Input),
                         ParameterInfo.From("Machinenumber", "<<!?TER-04523>>", ParameterDirection.Input)
                 },
                 new ParameterInfo[] { });

                yield return new TestCaseData(
                "@TransactionDateUtcFrom=2021-03-18T08:08:08Z567",
                new ParameterInfo[]
                {
                         ParameterInfo.From("TransactionDateUtcFrom", "2021-03-18T08:08:08Z567", ParameterDirection.Input)
                },
                new ParameterInfo[] { });

                yield return new TestCaseData(
               "@MandatorNumber=9, @TransactionDateUtcFrom=2021-03-18T08:08:08Z567",
               new ParameterInfo[]
               {
                         ParameterInfo.From("MandatorNumber", "9", ParameterDirection.Input),    
                         ParameterInfo.From("TransactionDateUtcFrom", "2021-03-18T08:08:08Z567", ParameterDirection.Input)
               },
               new ParameterInfo[] { });

                yield return new TestCaseData(
               "@MandatorNumber=9, @TransactionDateUtcFrom=2021-03-18T08:08:08Z567",
               new ParameterInfo[]
               {
                    ParameterInfo.From("MandatorNumber", "9", ParameterDirection.Input),
                    ParameterInfo.From("TransactionDateUtcFrom", "2021-03-18T08:08:08Z567", ParameterDirection.Input)
               },
               new ParameterInfo[] { });

               yield return new TestCaseData(
               "@county=Kärnten, @TransactionDateUtcFrom=2021-03-18T08:08:08Z567",
               new ParameterInfo[]
               {
                    ParameterInfo.From("county", "Kärnten", ParameterDirection.Input),
                    ParameterInfo.From("TransactionDateUtcFrom", "2021-03-18T08:08:08Z567", ParameterDirection.Input)
               },
               new ParameterInfo[] { });

               yield return new TestCaseData(
               "@country=Österreich, @TransactionDateUtcFrom=2021-03-18T08:08:08Z567",
               new ParameterInfo[]
               {
                    ParameterInfo.From("country", "Österreich", ParameterDirection.Input),
                    ParameterInfo.From("TransactionDateUtcFrom", "2021-03-18T08:08:08Z567", ParameterDirection.Input)
               },
               new ParameterInfo[] { });

               yield return new TestCaseData(
               "@messageWithSpecialVocals=ÜÖÄüÄöü, @TransactionDateUtcFrom=2021-03-18T08:08:08Z567",
               new ParameterInfo[]
               {
                    ParameterInfo.From("messageWithSpecialVocals", "ÜÖÄüÄöü", ParameterDirection.Input),
                    ParameterInfo.From("TransactionDateUtcFrom", "2021-03-18T08:08:08Z567", ParameterDirection.Input)
               },
               new ParameterInfo[] { });
            }
        }
    }
}
