using Microsoft.Extensions.Configuration;
using NServiceBus;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Shipping
{
    class Program
    {
        public static IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.Development.json", false)
                .Build();

        static async Task Main()
        {
            Console.Title = "Shipping";

            var config = new EndpointConfiguration("RetailDemo.Shipping");
            config.AssemblyScanner();
            config.LimitMessageProcessingConcurrencyTo(1);
            config.UseSerialization<NewtonsoftSerializer>();
            config.EnableInstallers();

            config.SendFailedMessagesTo("error");
            config.AuditProcessedMessagesTo("audit");

            var metrics = config.EnableMetrics();
            metrics.SendMetricDataToServiceControl(
                serviceControlMetricsAddress: "Particular.Monitoring",
                interval: TimeSpan.FromSeconds(2)
            );

            var persistence = config.UsePersistence<AzureTablePersistence>();
            persistence.ConnectionString(Configuration.GetConnectionString("StorageAccountConnectionString"));

            var transport = config.UseTransport<AzureServiceBusTransport>();
            transport.ConnectionString(Configuration.GetConnectionString("ServiceBusConnectionString"));
            transport.EnablePartitioning();

            var endpointInstance = await Endpoint.Start(config)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
