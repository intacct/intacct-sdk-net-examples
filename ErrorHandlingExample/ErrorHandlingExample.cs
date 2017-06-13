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
 * 1. Catch a ResponseException for an invalid login due to an incorrect Web Services sender ID or password.
 * 2. Catch an OperationException for an invalid login due to an incorrect company user ID or password.
 * 3. Catch a ResultException for an aborted transaction.
 *
 *  See https://developer.intacct.com/tools/sdk-net/errors-example/
 *  for detailed instructions on running this example.
 */

using System.Configuration;
using Intacct.Sdk;
using Intacct.Sdk.Xml;
using Intacct.Sdk.Exceptions;
using Intacct.Sdk.Functions.Common;
using System;
using System.Threading.Tasks;

namespace ErrorHandlingExample
{
    class ErrorHandlingExample
    {
        static void Main(string[] args)
        {
            /*
             * Example 1 for invalid Web Services sender password. ResponseException is thrown.
             * Uses the 'wrong_sender_password' profile in the .ini file.
             */
            try
            {
                SdkConfig config = new SdkConfig()
                {
                    SenderId = ConfigurationManager.AppSettings.Get("sender_id"),
                    SenderPassword = "wrong_sender_password",
                    CompanyId = ConfigurationManager.AppSettings.Get("company_id"),
                    UserId = ConfigurationManager.AppSettings.Get("user_id"),
                    UserPassword = ConfigurationManager.AppSettings.Get("user_password"),
                };

                IntacctClient client = new IntacctClient(config);

            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    // Handle the Response exception.
                    if (e is ResponseException)
                    {
                        // Exceptions are printed for demonstration purposes only -- more error handling is needed in
                        // production code.
                        Console.WriteLine("Example 1, " + e.Message + "\n");

                        foreach (String error in ((ResponseException)e).Errors)
                        {
                            Console.WriteLine(error + "\n"); // Print the error message.
                        }
                    }
                    // Rethrow any other exception.
                    else
                    {
                        throw;
                    }
                }
            }

            /*
             * Example 2 for invalid user password.  OperationException is thrown.
             * Uses the 'wrong_user_password' profile in the .ini file.
             */
            try
            {
                SdkConfig config = new SdkConfig()
                {
                    SenderId = ConfigurationManager.AppSettings.Get("sender_id"),
                    SenderPassword = ConfigurationManager.AppSettings.Get("sender_password"),
                    CompanyId = ConfigurationManager.AppSettings.Get("company_id"),
                    UserId = ConfigurationManager.AppSettings.Get("user_id"),
                    UserPassword = "wrong_user_password",
                };

                IntacctClient client = new IntacctClient(config);

            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    // Handle the Operation exception.
                    if (e is OperationException)
                    {
                        // Exceptions are printed for demonstration purposes only -- more error handling is needed in
                        // production code.
                        Console.WriteLine("Example 2, " + e.Message + "\n");

                        foreach (String error in ((OperationException)e).Errors)
                        {
                            Console.WriteLine(error + "\n"); // Print the error message.
                        }
                    }
                    // Rethrow any other exception.
                    else
                    {
                        throw;
                    }
                }
            }

            /*
             * Example 3 for no data returned in query.  ResultException is thrown.
             * Uses the default profile in the .ini file.
             */
            try
            {
                SdkConfig config = new SdkConfig()
                {
                    SenderId = ConfigurationManager.AppSettings.Get("sender_id"),
                    SenderPassword = ConfigurationManager.AppSettings.Get("sender_password"),
                    CompanyId = ConfigurationManager.AppSettings.Get("company_id"),
                    UserId = ConfigurationManager.AppSettings.Get("user_id"),
                    UserPassword = ConfigurationManager.AppSettings.Get("user_password"),
                };

                IntacctClient client = new IntacctClient(config);

                ReadByName readByName = new ReadByName(); 

                readByName.ObjectName = "GLENTRIES"; // Typo on 'GLENTRY'
                
                Content content = new Content();
                content.Add(readByName);

                // Call the client instance to execute the Content.
                Task<SynchronousResponse> response = client.Execute(content, true, "", false, null); // Second param is true for a transaction

                // No error thrown yet. Was the transaction successful?
                response.Result.Operation.Results[0].EnsureStatusSuccess();

            }
            catch (ResultException e)
            {

                // Exceptions are printed for demonstration purposes only -- more error handling is needed in
                // production code.
                Console.WriteLine("Example 3, " + e.Message + "\n");

                foreach (String error in ((ResultException)e).Errors)
                {
                    Console.WriteLine(error + "\n"); // Print the error message.
                }
            }
            Console.WriteLine("Press any key to close");
            Console.ReadLine();
        }
    }
}
