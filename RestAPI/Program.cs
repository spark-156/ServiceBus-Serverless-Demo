using DataModel.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseNServiceBus(context =>
                {
                    string connectionString = context.Configuration.GetConnectionString("ServiceBusConnectionString"); 

                    var config = new EndpointConfiguration("RetailDemo.RestAPI");
                    config.AssemblyScanner();
                    config.UseSerialization<NewtonsoftSerializer>();
                    config.SendFailedMessagesTo("error");
                    config.AuditProcessedMessagesTo("audit");

                    var transport = config.UseTransport<AzureServiceBusTransport>();
                    transport.ConnectionString(connectionString);
                    transport.EnablePartitioning();

                    var routing = transport.Routing();
                    routing.RouteToEndpoint(typeof(PlaceOrderV1), "RetailDemo.Sales");
                    routing.RouteToEndpoint(typeof(PlaceOrderV2), "RetailDemo.Sales");

                    config.SendOnly();

                    return config;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
