using System;
using System.Threading.Tasks;
using Intacct.SDK;
using Intacct.SDK.Xml;
using Intacct.SDK.Xml.Response;
using NLog;

namespace Intacct.Examples
{
    public static class CustomerObjectFunction
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