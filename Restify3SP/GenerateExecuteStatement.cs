using System;
using System.Text.RegularExpressions;

namespace Restify3SP
{
    public static class GenerateExecuteStatement
    {
        private static Regex splitProcedureNameFromArguments = 
            new Regex("([a-zA-Z0-9]*)\\((([a-zA-Z0-9 \\-\']+,?)*)\\)");
        private static Regex formatParameter =
            new Regex("([a-zA-Z0-9]+ )");

        public static string FromSubroute(string shema, string route)
        {
            var match = splitProcedureNameFromArguments.Match(route);
            string parameter = String.Empty;
            if (match.Groups.Count > 1 && match.Groups[2].Value != String.Empty)
            {
                parameter = $" {formatParameter.Replace(match.Groups[2].Value, "@$1= ")}";
            }
            return $"exec {shema}.{match.Groups[1]}{parameter}";
        }
    }
}
