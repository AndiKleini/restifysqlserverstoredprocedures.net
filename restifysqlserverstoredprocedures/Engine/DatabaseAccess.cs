using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using static Dapper.SqlMapper;

namespace restifysqlserverstoredprocedures.Engine
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
        public async Task<QueryResult> ExecuteWithParameter(string procedure, EnrichedDynamicParameters parameters)
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
                            this.AllResultsetsOf(reader).
                            Select(async s => await reader.ReadAsync<object>()).ToArray());
                }
               
                return new QueryResult()
                {
                    Result = tmpResults,
                    OutputParameter = parameters.GetParameterForDirection(ParameterDirection.Output),
                    Return = parameters.Get<int>(returnParameterName)
                };
            }
        }

        private IEnumerable<bool> AllResultsetsOf(GridReader reader)
        {
            while (!reader.IsConsumed)
            {
                yield return true;
            }
            yield break;
        }
    }
}
