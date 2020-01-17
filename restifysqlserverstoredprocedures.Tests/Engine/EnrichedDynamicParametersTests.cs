using FluentAssertions;
using NUnit.Framework;
using restifysqlserverstoredprocedures.Engine;
using restifysqlserverstoredprocedures.Tests.Engine.Help;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace restifysqlserverstoredprocedures.Tests.Engine
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

                // yield return new TestCaseData("@language = 'en'", new Tuple<string, object>[] { new Tuple<string, object>("@language", "en") });
                /* 
                 yield return new TestCaseData(new Tuple<string, object>[] { new Tuple<string, object>("@number", 12) });
                 yield return new TestCaseData(new Tuple<string, object>[] { new Tuple<string, object>("@language", "en") });
                 yield return new TestCaseData(new Tuple<string, object>[] { new Tuple<string, object>("@language", "en") });
                 yield return new TestCaseData(new Tuple<string, object>[] { new Tuple<string, object>("@language", "en") });
                 yield return new TestCaseData(new Tuple<string, object>[] { new Tuple<string, object>("@language", "en") });
                 */
            }
        }




    }
}
