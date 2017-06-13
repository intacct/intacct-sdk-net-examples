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
 *  This example shows you how to do the following:
 *  1. Create an IntacctClient based on a credentials file.
 *  2. Use instances of SDK classes to create one CUSTOMER object and update another.
 *  3. Wrap these operations in Content instance.
 *  4. Execute the request and send the Content to the gateway.
 *
 *  Note: An example that shows how to catch useful error info if using
 *  a transaction is also provided but commented out.
 */

using Intacct.Sdk.Exceptions;
using Intacct.Sdk;
using Intacct.Sdk.Xml;
using Intacct.Sdk.Xml.Response.Operation;
using Intacct.Sdk.Functions.AccountsReceivable;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace CreateObjectExample
{
    class CreateObjectExample
    {
        static void Main(string[] args)
        {
            // Wrap your calls in a try block to support error handling.
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

                // Create CUSTOMER objects.
                CustomerCreate customerCreate = new CustomerCreate();
                customerCreate.CustomerName = "Joshua Granley";

                // Update CUSTOMER object
                CustomerUpdate customerUpdate = new CustomerUpdate();
                customerUpdate.CustomerId = "10055";  // Update with valid Customer ID!
                customerUpdate.Comments = "Gold star customer!";

                Content content = new Content();
                content.Add(customerCreate);
                content.Add(customerUpdate);

                // Call the client instance to execute the Content.
                Task<SynchronousResponse> response = client.Execute(content);

                response.Wait();

                // Iterate response
                foreach (Result result in response.Result.Operation.Results)
                {
                    Console.WriteLine(result.Data);
                }
            }
            catch (ResultException e) {
                Console.WriteLine(e);
            } catch (ResponseException e) {
                Console.WriteLine(e);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("\nPress any key to close");
            Console.ReadLine();
        }
    }
}
