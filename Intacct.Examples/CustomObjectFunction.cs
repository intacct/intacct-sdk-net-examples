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
using Intacct.SDK.Xml;
using Intacct.SDK.Xml.Response;
using NLog;

namespace Intacct.Examples
{
    public static class CustomObjectFunction
    {
        public static void Run(ILogger logger)
        {
            OnlineClient client = Bootstrap.Client(logger);

            logger.Info("Executing create test object function to API");
            
            TestObjectCreate create = new TestObjectCreate()
            {
                Name = "hello world",
            };

            Task<OnlineResponse> createTask = client.Execute(create);
            createTask.Wait();
            OnlineResponse createResponse = createTask.Result;
            Result createResult = createResponse.Results[0];

            int recordNo = int.Parse(createResult.Data[0].Element("id").Value);
            
            Console.WriteLine("Created record ID " + recordNo.ToString());
        }
    }
}