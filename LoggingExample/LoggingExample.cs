/*
 * Copyright 2017 Intacct Corporation.
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

/*
*  This example shows how to do the following:
*  1. Create a logger using NLog (third-party logger).
*  2. Pass that logger to an IntacctClient instance.
*  3. Execute a request that will throw some errors.
*  4. Check transaction success, catch errors, and log them.
*
*  Prerequisites:
*  - You have a working knowledge of C#/.NET.
*  - You meet the system requirements for the Intacct SDK for .NET.
*  - You have a C# IDE as well as the NuGet package manager.
*  - You have installed the .NET SDK and its dependencies using Composer.
*
 *  See https://developer.intacct.com/tools/sdk-net/logging-example/
 *  for detailed instructions on meeting the prerequisites and running this example.
*/

using System.Configuration;
using Intacct.Sdk;
using Intacct.Sdk.Xml;
using Intacct.Sdk.Functions.Common;
using System;
using System.Threading.Tasks;
using NLog;

namespace intacct_sdk_net_examples
{
    class LoggingExample
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
           
            try
            {
                
                // Get the keys from the Credentials file. For illustrative purposes, these are coming from config file
                SdkConfig config = new SdkConfig()
                {
                    SenderId = ConfigurationManager.AppSettings.Get("sender_id"),
                    SenderPassword = ConfigurationManager.AppSettings.Get("sender_password"),
                    CompanyId = ConfigurationManager.AppSettings.Get("company_id"),
                    UserId = ConfigurationManager.AppSettings.Get("user_id"),
                    UserPassword = ConfigurationManager.AppSettings.Get("user_password"),
                    Logger = logger,
                };

                IntacctClient client = new IntacctClient(config);

                ReadByName readByName = new ReadByName()
                {
                    ObjectName = "GLENTRIES",  //Use incorrect name (should be GLENTRY)
                };

                Content content = new Content();
                content.Add(readByName);

                Task<SynchronousResponse> response = client.Execute(content);

                response.Wait();

                // Just try to ensure the status is successful for the first result
                response.Result.Operation.Results[0].EnsureStatusSuccess();
            }
            catch (Exception ex)
            {
                logger.Error(ex); // Log the error messages.
            }

        }
    }
}
