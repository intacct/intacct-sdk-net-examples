using System;
using Intacct.SDK.Exceptions;
using Newtonsoft.Json;
using NLog;

namespace Intacct.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Pick an option to run:");
            Console.WriteLine(" 1 - Getting started");
            Console.WriteLine(" 2 - List vendors");
            Console.WriteLine(" 3 - CRUD customer");
            Console.WriteLine(" 4 - Custom object function");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                ILogger logger = Bootstrap.Logger();
                
                try
                {
                    try
                    {
                        switch (option)
                        {
                            case 1:
                                GettingStarted.Run(logger);
                                break;
                            case 2:
                                ListVendors.Run(logger);
                                break;
                            case 3:
                                CrudCustomer.Run(logger);
                                break;
                            case 4:
                                CustomerObjectFunction.Run(logger);
                                break;
                            default:
                                Console.WriteLine("Invalid option entered");
                                break;
                        }
                    }
                    catch (AggregateException e)
                    {
                        foreach (var ie in e.Flatten().InnerExceptions)
                        {
                            if (ie is ResponseException ex)
                            {
                                logger.Error(
                                    "An Intacct response exception was thrown [ Class={0}, Message={1}, API Errors={2} ]",
                                    ie.GetType(),
                                    ex.Message,
                                    JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ex.Errors))
                                );
                                Console.WriteLine("Failed! " + ie.Message);
                            }
                            else
                            {
                                throw ie;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.Error(
                        "An exception was thrown [ Class={0}, Message={1} ]",
                        e.GetType(),
                        e.Message
                    );
                    Console.WriteLine(e.GetType() + ": " + e.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid option entered");
            }
        }
    }
}