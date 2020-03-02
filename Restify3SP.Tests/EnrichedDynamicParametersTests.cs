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
