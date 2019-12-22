using System.Collections.Generic;
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
    }
}
