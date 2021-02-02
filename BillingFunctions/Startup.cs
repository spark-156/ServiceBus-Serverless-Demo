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
            builder.UseNServiceBus(() => new ServiceBusTriggeredEndpointConfiguration("RetailDemo.Billing", "ServiceBusConnectionString"));
        }
    }
}
