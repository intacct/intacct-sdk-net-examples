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
using System.Collections.Generic;
using System.Threading.Tasks;
using Intacct.SDK;
using Intacct.SDK.Functions.Common.NewQuery;
using Intacct.SDK.Functions.Common.NewQuery.QueryFilter;
using Intacct.SDK.Functions.Common.NewQuery.QueryOrderBy;
using Intacct.SDK.Functions.Common.NewQuery.QuerySelect;
using Intacct.SDK.Xml;
using Intacct.SDK.Xml.Response;
using Newtonsoft.Json;
using NLog;

namespace Intacct.Examples
{
    public static class Query
    {
        public static void Run(ILogger logger)
        {
            OnlineClient client = Bootstrap.Client(logger);

            List<IFilter> filterList = new List<IFilter>();
            filterList.Add((new Filter("CUSTOMERID")).SetLike("c%"));
            filterList.Add((new Filter("CUSTOMERID")).SetLike("1%"));
            OrOperator filter = new OrOperator(filterList);
            
            OrderBuilder orderBuilder = new OrderBuilder();
            IOrder[] orders = orderBuilder.Descending("CUSTOMERID").GetOrders();

            SelectBuilder selectBuilder = new SelectBuilder();
            ISelect[] fields = selectBuilder.
                Fields(new[] {"CUSTOMERID","CUSTOMERNAME"}).
                Sum("TOTALDUE").
                GetFields();
            
            QueryFunction query = new QueryFunction()
            {
                SelectFields = fields,
                ObjectName = "ARINVOICE",
                Filter =  filter,
                CaseInsensitive = true,
                PageSize = 100,
                OrderBy = orders
            };

            logger.Info("Executing query to Intacct API");

            Task<OnlineResponse> task = client.Execute(query);
            task.Wait();
            OnlineResponse response = task.Result;
            Result result = response.Results[0];
            
            dynamic json = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(result.Data));

            if (json != null && json.First != null)
            {
                Console.WriteLine("Success! Number of ARINVOICE objects found: " + result.TotalCount);
                Console.WriteLine("First ARINVOICE result found");
                Console.WriteLine("CUSTOMERID: " + json.First["ARINVOICE"]["CUSTOMERID"].Value);
                Console.WriteLine("CUSTOMERNAME: " + json.First["ARINVOICE"]["CUSTOMERNAME"].Value);
                Console.WriteLine("SUM.TOTALDUE: " + json.First["ARINVOICE"]["SUM.TOTALDUE"].Value);
                
                Console.WriteLine("See the log file (logs/intacct.html) for the complete list of results.");
            }
            else
            {
                Console.WriteLine("The query executed, but no ARINVOICE objects met the query criteria.");
                Console.WriteLine("Either modify the filter or comment it out from the query.");
                Console.WriteLine("See the log file (logs/intacct.html) for the XML request.");
            }
            
            logger.Debug(
                "Query successful [ Company ID={0}, User ID={1}, Request control ID={2}, Function control ID={3}, Total count={4}, Data={5} ]",
                response.Authentication.CompanyId,
                response.Authentication.UserId,
                response.Control.ControlId,
                result.ControlId,
                result.TotalCount,
                JsonConvert.DeserializeObject(JsonConvert.SerializeObject(result.Data))
            );

        }
    }
}