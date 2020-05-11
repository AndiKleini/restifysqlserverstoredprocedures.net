using System.Net;

namespace Restify3SP.IntegrationTests.Engine
{
    internal static class ScriptRunnerRepository
    {
        private static ScriptRunner instance;
        public static ScriptRunner GetInstance()
        {
            return instance;
        }
        public static void SetInstance(ScriptRunner runner)
        {
            instance = runner;
        }
    }
}
