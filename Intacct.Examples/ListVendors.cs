﻿/*
 * Copyright 2018 Sage Intacct, Inc.
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
    public static class ListVendors
    {
        public static void Run(ILogger logger)
        {
            OnlineClient client = Bootstrap.Client(logger);

            ReadByQuery query = new ReadByQuery()
            {
                ObjectName = "VENDOR",
                PageSize = 2, // Keep the count to just 2 for the example
                Fields =
                {
                    "RECORDNO",
                    "VENDORID",
                }
            };

            logger.Info("Executing query to Intacct API");

            Task<OnlineResponse> task = client.Execute(query);
            task.Wait();
            OnlineResponse response = task.Result;
            Result result = response.Results[0];

            logger.Debug(
                "Query successful - page 1 [ Total count={0}, Data={1} ]",
                result.TotalCount,
                JsonConvert.DeserializeObject(JsonConvert.SerializeObject(result.Data))
            );

            Console.WriteLine("Page 1 success! Number of vendor objects found: " + result.TotalCount + ". Number remaining: " + result.NumRemaining);

            int i = 1;
            while (result.NumRemaining > 0 && i <= 3 && !string.IsNullOrEmpty(result.ResultId))
            {
                i++;
                ReadMore more = new ReadMore()
                {
                    ResultId = result.ResultId
                };
                
                Task<OnlineResponse> taskMore = client.Execute(more);
                taskMore.Wait();
                OnlineResponse responseMore = taskMore.Result;
                Result resultMore = responseMore.Results[0];

                logger.Debug(
                    "Read More successful - page " + i + " [ Total remaining={0}, Data={1} ]",
                    resultMore.NumRemaining,
                    JsonConvert.DeserializeObject(JsonConvert.SerializeObject(resultMore.Data))
                );

                Console.WriteLine("Page " + i + " success! Records remaining: " + resultMore.NumRemaining);
            }
            
            Console.WriteLine("Successfully read " + i + " pages");
        }
    }
}