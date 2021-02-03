using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(SalesFunctions.Startup))]
namespace SalesFunctions
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.UseNServiceBus(() =>
            {
                var config = new ServiceBusTriggeredEndpointConfiguration("RetailDemo.Sales", "ServiceBusConnectionString");
                config.AdvancedConfiguration.SendFailedMessagesTo("error");
                config.AdvancedConfiguration.AuditProcessedMessagesTo("audit");

                var metrics = config.AdvancedConfiguration.EnableMetrics();

                metrics.SendMetricDataToServiceControl(
                    serviceControlMetricsAddress: "Particular.Monitoring",
                    interval: TimeSpan.FromSeconds(2)
                );
                return config;
            });
            builder.Services.AddHttpClient();
        }
    }
}
