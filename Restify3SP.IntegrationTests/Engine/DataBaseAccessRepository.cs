namespace Restify3SP.IntegrationTests.Engine
{
    public static class DataBaseAccessRepository
    {
        private static DatabaseAccess access;

        public static void SetConnectionString(string connectionString)
        {
            access = new DatabaseAccess(connectionString);
        }

        public static DatabaseAccess GetInstance()
        {
            return access;
        }
    }
}
