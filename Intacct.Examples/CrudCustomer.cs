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

using System;
using System.Threading.Tasks;
using Intacct.SDK;
using Intacct.SDK.Functions.AccountsReceivable;
using Intacct.SDK.Functions.Common;
using Intacct.SDK.Xml;
using Intacct.SDK.Xml.Response;
using Microsoft.Extensions.Logging;
using NLog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Intacct.Examples
{
    public static class CrudCustomer
    {
        public static void Run(ILogger logger)
        {
            OnlineClient client = Bootstrap.Client(logger);

            logger.LogInformation("Executing CRUD customer functions to API");
            
            CustomerCreate create = new CustomerCreate()
            {
                CustomerName = "Joshua Granley",
                Active = false,
            };

            Task<OnlineResponse> createTask = client.Execute(create);
            createTask.Wait();
            OnlineResponse createResponse = createTask.Result;
            Result createResult = createResponse.Results[0];

            string customerId = createResult.Data[0].Element("CUSTOMERID").Value;
            int recordNo = int.Parse(createResult.Data[0].Element("RECORDNO").Value);
            
            Console.WriteLine("Created inactive customer ID " + customerId);
            
            CustomerUpdate update = new CustomerUpdate()
            {
                CustomerId = customerId,
                Active = true,
            };

            Task<OnlineResponse> updateTask = client.Execute(update);
            updateTask.Wait();
            
            Console.WriteLine("Updated customer ID " + customerId + " to active");
            
            Read read = new Read()
            {
                ObjectName = "CUSTOMER",
                Fields = {
                    "RECORDNO",
                    "CUSTOMERID",
                    "STATUS",
                },
                Keys = {
                    recordNo,
                }
            };

            Task<OnlineResponse> readTask = client.Execute(read);
            readTask.Wait();
            
            Console.WriteLine("Read customer ID " + customerId);
            
            CustomerDelete delete = new CustomerDelete()
            {
                CustomerId = customerId,
            };

            Task<OnlineResponse> deleteTask = client.Execute(delete);
            deleteTask.Wait();
            
            Console.WriteLine("Deleted customer ID " + customerId);
            
            LogManager.Flush();
        }
    }
}