/*
 * Copyright 2020 Sage Intacct, Inc.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"). You may not
 * use this file except in compliance with the License. You may obtain a copy 
 * of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * or in the "LICENSE" file accompanying this file. This file is distributed on 
 * an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either 
 * express or implied. See the License for the specific language governing 
 * permissions and limitations under the License.
 */

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