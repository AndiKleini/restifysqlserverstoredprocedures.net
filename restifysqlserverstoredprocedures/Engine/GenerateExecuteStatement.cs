using System;
using System.Text.RegularExpressions;

namespace restifysqlserverstoredprocedures.Engine
{
    public static class GenerateExecuteStatement
    {
        private static Regex splitProcedureNameFromArguments = 
            new Regex("([a-zA-Z0-9]*)/([a-zA-Z0-9]*)\\((([a-zA-Z0-9 \\-\']+,?)*)\\)");
        private static Regex formatParameter =
            new Regex("([a-zA-Z0-9]+ )");

        public static string FromSubroute(string route)
        {
            var match = splitProcedureNameFromArguments.Match(route);
            string parameter = String.Empty;
            if (match.Groups.Count > 2 && match.Groups[3].Value != String.Empty)
            {
                parameter = $" {formatParameter.Replace(match.Groups[3].Value, "@$1= ")}";
            }
            return $"exec {match.Groups[1]}.{match.Groups[2]}{parameter}";
        }

        /* static formatParameters = new RegExp('([a-zA-Z0-9]+ )', 'g');
        static fromQueryString(querystring: string) : string { 
        const match = this.splitProcedureNameFromArguments.exec(querystring);
        let parameter = stringEmpty;
        if (match[3] !== stringEmpty) {
            parameter = ` ${match[3].replace(this.formatParameters, '@$1= ')}`;
        }
        return `exec ${match[1]}.${match[2]}${parameter}`;
        */
    }
}
