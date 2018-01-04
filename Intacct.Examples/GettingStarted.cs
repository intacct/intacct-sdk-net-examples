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

            ReadByQuery query = new ReadByQuery()
            {
                ObjectName = "VENDOR",
                PageSize = 1, // Keep the count to just 1 for the example
                Fields =
                {
                    "RECORDNO",
                    "VENDORID",
                    "NAME"
                }
            };

            logger.Info("Executing query to Intacct API");

            Task<OnlineResponse> task = client.Execute(query);
            task.Wait();
            OnlineResponse response = task.Result;
            Result result = response.Results[0];

            logger.Debug(
                "Query successful [ Company ID={0}, User ID={1}, Request control ID={2}, Function control ID={3}, Total count={4}, Data={5} ]",
                response.Authentication.CompanyId,
                response.Authentication.UserId,
                response.Control.ControlId,
                result.ControlId,
                result.TotalCount,
                JsonConvert.DeserializeObject(JsonConvert.SerializeObject(result.Data))
            );

            Console.WriteLine("Success! Number of vendor objects found: " + result.TotalCount);
        }
    }
}