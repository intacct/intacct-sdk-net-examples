using System;
using System.Threading.Tasks;
using Intacct.SDK;
using Intacct.SDK.Functions.AccountsReceivable;
using Intacct.SDK.Functions.Common;
using Intacct.SDK.Xml;
using Intacct.SDK.Xml.Response;
using NLog;

namespace Intacct.Examples
{
    public static class CrudCustomer
    {
        public static void Run(ILogger logger)
        {
            OnlineClient client = Bootstrap.Client(logger);

            logger.Info("Executing CRUD customer functions to API");
            
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
        }
    }
}