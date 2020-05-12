using Dapper;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Restify3SP.IntegrationTests.Engine
{
    public class ScriptRunner
    {
        private string connectionString;
        private SqlConnection connection;

        public ScriptRunner(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<int> ExecuteScriptFromFile(string path)
        {
            return await this.ExecuteScriptFromFile(new FileInfo(path));
        }

        public async Task<int> ExecuteScriptFromFile(FileInfo file)
        {
            if (this.connection == null || 
                this.connection.State == System.Data.ConnectionState.Closed || 
                this.connection.State == System.Data.ConnectionState.Broken)
            {
                this.connection = new SqlConnection(this.connectionString);
            }

            return await this.connection.ExecuteAsync(File.ReadAllText(file.FullName));
        }

        public async Task<int> ExecuteAllScriptsInPath(string path)
        {
            var directory = new DirectoryInfo(path);
            foreach(var file in directory.GetFiles())
            {
                int returnCode = 0;
                if ((returnCode = await this.ExecuteScriptFromFile(file)) != 0)
                {
                    return returnCode;
                }
            }
            return 0;
        }

        public void CloseConnection()
        {
            if (this.connection != null && 
                this.connection.State != System.Data.ConnectionState.Closed)
            {
                this.connection.Close();
            }
        }
    }
}
