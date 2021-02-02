using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DataModel.Commands;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.Storage.Blob;
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

        [FunctionName("OrderPlacedHandler")]
        public async Task Run(
        [ServiceBusTrigger(queueName: "RetailDemo.Billing", Connection = "ServiceBusConnectionString")]
            Message message,
            ILogger logger,
            ExecutionContext executionContext,
            IBinder binder)
        {
            await endpoint.Process(message, executionContext, logger);

            // determine the path at runtime in any way you choose
            string path = $"invoices/invoice-{Guid.NewGuid()}.txt";

            var attribute = new BlobAttribute(path, FileAccess.Write) { Connection = "StorageAccountConnectionString" };
            using (var writer = await binder.BindAsync<TextWriter>(attribute))
            {
                await writer.WriteAsync("Hello this is paid.");
            }
        }
    }
}
