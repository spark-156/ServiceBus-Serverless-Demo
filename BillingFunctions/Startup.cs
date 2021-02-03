using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(BillingFunctions.Startup))]
namespace BillingFunctions
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.UseNServiceBus(() =>
            {
                var config = new ServiceBusTriggeredEndpointConfiguration("RetailDemo.Billing", "ServiceBusConnectionString");
                config.AdvancedConfiguration.SendFailedMessagesTo("error");
                config.AdvancedConfiguration.AuditProcessedMessagesTo("audit");

                var metrics = config.AdvancedConfiguration.EnableMetrics();

                metrics.SendMetricDataToServiceControl(
                    serviceControlMetricsAddress: "Particular.Monitoring",
                    interval: TimeSpan.FromSeconds(2)
                );
                return config;
            });
        }
    }
}
