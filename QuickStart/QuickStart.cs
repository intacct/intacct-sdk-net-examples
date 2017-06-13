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
*  This example shows you how to do the following:
*  1. Open a session by creating an IntacctClient and providing login credentials
*     loaded Application Configuration Management.
*  3. Configure a Read call on VENDOR objects and wrap this in a Content object.
*  4. Use the IntacctClient instance to execute a request with the Content.
*  5. Get a count of VENDOR objects from the returned result.
*
*  Prerequisites:
*  - You have a working knowledge of C#/.NET.
*  - You meet the system requirements for the Intacct SDK for .NET.
*  - You have a C# IDE as well as the NuGet package manager.
*  - You have installed the .NET SDK and its dependencies using Composer.
*
*  See https://developer.intacct.com/tools/sdk-net/getting-started/
*  for detailed instructions on meeting the prerequisites and running this example.
*/
using System.Configuration;
using Intacct.Sdk;
using Intacct.Sdk.Xml;
using Intacct.Sdk.Functions.Common;
using System;
using System.Threading.Tasks;

namespace QuickStart
{
    class QuickStart
    {
        static void Main(string[] args)
        {

            try
            {

                // Get the keys from the Credentials file, for illustrative purposes, these are coming from config file
                SdkConfig config = new SdkConfig()
                {
                    SenderId = ConfigurationManager.AppSettings.Get("sender_id"),
                    SenderPassword = ConfigurationManager.AppSettings.Get("sender_password"),
                    CompanyId = ConfigurationManager.AppSettings.Get("company_id"),
                    UserId = ConfigurationManager.AppSettings.Get("user_id"),
                    UserPassword = ConfigurationManager.AppSettings.Get("user_password"),
                };
                IntacctClient client = new IntacctClient(config);

                Console.WriteLine("Current Company ID: " + client.SessionCreds.CurrentCompanyId);
                Console.WriteLine("Current User ID: " + client.SessionCreds.CurrentUserId);

                Read read = new Read()
                {
                    ObjectName = "VENDOR",
                };
                Content content = new Content();
                content.Add(read);

                Task<SynchronousResponse> response = client.Execute(content);

                response.Wait();

                Console.WriteLine("Read function control ID: " + response.Result.Control.ControlId);

                Console.WriteLine("Number of vendor objects read: " + response.Result.Operation.Results[0].Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught: " + ex.Message);
            }

            Console.WriteLine("\nPress any key to close");
            Console.ReadLine();
        }
    }
}
