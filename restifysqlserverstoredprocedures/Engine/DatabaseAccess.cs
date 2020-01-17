using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace restifysqlserverstoredprocedures.Engine
{
    public class DatabaseAccess
    {
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
            IEnumerable<object> tmpResult;
            parameters.AddParameter("Return", null, DbType.Int32, ParameterDirection.ReturnValue, 4);
            using (var connection = new SqlConnection(this.connectionString))
            {
                tmpResult = await connection.QueryAsync<object>(
                    procedure,
                    parameters, null, null, CommandType.StoredProcedure);
            }

            return new QueryResult()
            {
                Result = tmpResult,
                OutputParameter = parameters.GetParameterForDirection(ParameterDirection.Output),
                Return = parameters.Get<int>("Return")
            };
        }
    }
}
