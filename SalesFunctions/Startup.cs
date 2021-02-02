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
            builder.UseNServiceBus(() => new ServiceBusTriggeredEndpointConfiguration("RetailDemo.Sales", "ServiceBusConnectionString"));
            builder.Services.AddHttpClient();
        }
    }
}
