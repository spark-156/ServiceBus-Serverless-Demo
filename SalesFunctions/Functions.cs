using System;
using System.Threading.Tasks;
using DataModel.Commands;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace SalesFunctions
{
    public class Functions
    {
        private readonly IFunctionEndpoint endpoint;

        public Functions(IFunctionEndpoint endpoint)
        {
            this.endpoint = endpoint;
        }

        [FunctionName("PlaceOrderHandler")]
        public async Task Run(
            [ServiceBusTrigger(
                queueName: "RetailDemo.Sales",
                Connection = "ServiceBusConnectionString")
            ] Message message,
            ILogger logger,
            ExecutionContext executionContext)
        {
            await endpoint.Process(message, executionContext, logger);
        }
    }
}
