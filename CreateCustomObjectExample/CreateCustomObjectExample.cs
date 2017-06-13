/**
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

/**
 *  This example is composed of several files:
 *  1. The program that sends a request that creates a custom object in the Intacct system (this file).
 *  2. An abstract class for a custom object (AbstractMyCustomObject).
 *  3. A concrete class that extends the abstract class and implements the writeXml() function (MyCustomObjectCreate).
 */

using System;
using System.Configuration;
using System.Threading.Tasks;
using Intacct.Sdk;
using Intacct.Sdk.Xml;
using Intacct.Sdk.Xml.Response.Operation;


namespace CreateCustomObjectExample
{
    class CreateCustomObjectExample
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
                                
                // Create custom object.
                MyCustomObjectCreate customObject = new MyCustomObjectCreate();
                customObject.Name = "Test name";
                customObject.Description = "Test description";

                Content content = new Content();
                content.Add(customObject);
                
                Task<SynchronousResponse> response = client.Execute(content);

                response.Wait();

                foreach (Result result in response.Result.Operation.Results)
                {
                    if (result.Errors.Count > 0)
                    {
                        foreach (String error in result.Errors)
                        {
                            Console.WriteLine(error);
                        }
                    } else
                    {
                        Console.WriteLine(result.Data);
                    }
                }
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
