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
using Intacct.SDK.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using NLog.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Intacct.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = Bootstrap.Logger("Program");
            
            Program p  = new Program();
            p.DoMenu(logger);
        }

        public void DoMenu(ILogger logger)
        {

            Console.WriteLine("Available examples:");
            Console.WriteLine(" 1 - Getting started");
            Console.WriteLine(" 2 - List AR invoices");
            Console.WriteLine(" 3 - List vendors (legacy)");
            Console.WriteLine(" 4 - CRUD customer");
            Console.WriteLine(" 5 - Custom object function");
            Console.WriteLine(" 6 - Exit program");

            string option = "";
            while (option != "6")
            {
                Console.WriteLine("");
                Console.Write("Enter a number to run the example > ");
                option = Console.ReadLine()?.ToLower();

                try
                {
                    try
                    {
                        switch (option)
                        {
                            case "1":
                                GettingStarted.Run(logger);
                                break;
                            case "2":
                                Query.Run(logger);
                                break;
                            case "3":
                                ListVendors.Run(logger);
                                break;
                            case "4":
                                CrudCustomer.Run(logger);
                                break;
                            case "5":
                                CustomObjectFunction.Run(logger);
                                break;
                            case "6":
                                Console.WriteLine("Exiting...");
                                LogManager.Shutdown();
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
                                logger.LogCritical(
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
                    logger.LogCritical(
                        "An exception was thrown [ Class={0}, Message={1} ]",
                        e.GetType(),
                        e.Message
                    );
                    Console.WriteLine(e.GetType() + ": " + e.Message);
                }
                finally
                {
                    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                    LogManager.Flush();
                }
            }
        }
    }
}