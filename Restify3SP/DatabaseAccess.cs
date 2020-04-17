using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using static Dapper.SqlMapper;

namespace Restify3SP
{
    public class DatabaseAccess
    {
        private const string returnParameterName = "Return";
        private string connectionString;
        public DatabaseAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<IEnumerable<object>> Execute(string procedure)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                return await connection.QueryAsync<object>(
                    procedure, 
                    System.Data.CommandType.StoredProcedure);
            }
        }
        public async Task<QueryResult> ExecuteWithParameter(string procedure, string parameters)
        {
            return await this.ExecuteWithParameter(procedure, EnrichedDynamicParameters.FromArguments(parameters));
        }
        private async Task<QueryResult> ExecuteWithParameter(string procedure, EnrichedDynamicParameters parameters)
        {
            IEnumerable<object>[] tmpResults;
            parameters.AddParameter(
                returnParameterName, 
                null, 
                DbType.Int32, 
                ParameterDirection.ReturnValue, 
                4);
            using (var connection = new SqlConnection(this.connectionString))
            {
                using (var reader = await connection.QueryMultipleAsync(
                    procedure,
                    parameters,
                    null,
                    null,
                    CommandType.StoredProcedure))
                {
                    tmpResults =
                        await Task.WhenAll(
                            this.EnumerateAllResultsetsOf(reader).
                            Select(async s => await reader.ReadAsync<object>()));
               }
               
                return new QueryResult()
                {
                    Result = tmpResults,
                    OutputParameter = parameters.GetParameterForDirection(ParameterDirection.Output),
                    Return = parameters.Get<int>(returnParameterName)
                };
            }
        }

        private IEnumerable<bool> EnumerateAllResultsetsOf(GridReader reader)
        {
            // HasRows method is necessary when SP emits no resultsets
            // compare issue below:
            // https://github.com/StackExchange/Dapper/issues/327
            while (!reader.IsConsumed && this.HasRows(reader))
            {
                yield return true;
            }
            yield break;
        }

        private bool HasRows(GridReader reader)
        {
            SqlDataReader internalReader = (SqlDataReader)typeof(GridReader).
                GetField
                ("reader",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(reader);
            return internalReader.HasRows;
        }
    }
}
