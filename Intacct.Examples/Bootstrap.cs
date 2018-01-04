using System.IO;
using Intacct.SDK;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Intacct.Examples
{
    public static class Bootstrap
    {
        public static ILogger Logger()
        {
            FileTarget target = new FileTarget
            {
                FileName = "${basedir}/logs/intacct.log"
            };
            SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);
            ILogger logger = LogManager.GetLogger("intacct-sdk-net-examples");
            
            return logger;
        }

        public static OnlineClient Client(ILogger logger)
        {
            ClientConfig clientConfig = new ClientConfig()
            {
                ProfileFile = Path.Combine(Directory.GetCurrentDirectory(), "credentials.ini"),
                Logger = logger,
            };
            OnlineClient client = new OnlineClient(clientConfig);
            
            return client;
        }
    }
}