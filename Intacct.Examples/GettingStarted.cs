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
using Intacct.SDK.Functions.Common;
using Intacct.SDK.Xml;
using Intacct.SDK.Xml.Response;
using Newtonsoft.Json;
using NLog;

namespace Intacct.Examples
{
    public static class GettingStarted
    {
        public static void Run(ILogger logger)
        {
            OnlineClient client = Bootstrap.Client(logger);

            Read read = new Read()
            {
                ObjectName = "CUSTOMER",
                Fields = {
                    "RECORDNO",
                    "CUSTOMERID",
                    "NAME",
                },
                Keys = {
                    33 // Replace with the record number of a customer in your company
                }
            };

            logger.Info("Executing read to Intacct API");

            Task<OnlineResponse> task = client.Execute(read);
            task.Wait();
            
            OnlineResponse response = task.Result;
            Result result = response.Results[0];

            dynamic json = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(result.Data));
            
            logger.Debug(
                "Query successful [ Company ID={0}, User ID={1}, Request control ID={2}, Function control ID={3}, Total count={4}, Data={5} ]",
                response.Authentication.CompanyId,
                response.Authentication.UserId,
                response.Control.ControlId,
                result.ControlId,
                result.TotalCount,
                json
            );

            Console.WriteLine("Success! Found these customers: " + json);
        }
    }
}