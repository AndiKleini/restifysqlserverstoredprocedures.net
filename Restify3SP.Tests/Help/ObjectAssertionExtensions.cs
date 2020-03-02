using FluentAssertions;
using FluentAssertions.Primitives;
using Restify3SP;
using System.Data;
using System.Linq;

namespace Restify3DS.Tests.Help
{
    public static class ObjectAssertionExtensions
    {
        public static void ContainParameters(
            this ObjectAssertions assertion,
            ParameterInfo[] inputParameterInfos = null, 
            ParameterInfo[] outputParameterInfos = null)
        {
            assertion.Subject.Should().BeOfType<EnrichedDynamicParameters>();
            var parametersToAssert = assertion.Subject as EnrichedDynamicParameters;
            if (inputParameterInfos != null)
            {
                parametersToAssert.ShouldMatchParameters(ParameterDirection.Input, inputParameterInfos);

            }
            if (outputParameterInfos != null)
            {
                parametersToAssert.ShouldMatchParameters(ParameterDirection.Output, outputParameterInfos);
            }
        }

        private static void ShouldMatchParameters(
            this EnrichedDynamicParameters parametersToAssert,
            ParameterDirection direction,
            ParameterInfo[] parameterInfos)
        {
            var filteredInput = parametersToAssert.GetParameterForDirection(direction);
            if (filteredInput.Any()) 
            { 
                filteredInput.Should().ContainKeys(parameterInfos.Select(s => s.Name));
                filteredInput.Should().ContainValues(parameterInfos.Select(s => s.Value));
            }
            else
            {
                parameterInfos.Should().BeEmpty();
            }
        }
    }
}
