using System;
using System.Net.Http;
using System.Threading.Tasks;
using DataModel.Commands;
using DataModel.Events;
using NServiceBus;
using NServiceBus.Logging;

namespace SalesFunctions
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrderV1>, IHandleMessages<PlaceOrderV2>
    {
        public PlaceOrderHandler(HttpClient crmHttpClient)
        {
            this.crmHttpClient = crmHttpClient;
        }

        private static readonly ILog Log = LogManager.GetLogger<PlaceOrderHandler>();
        private readonly HttpClient crmHttpClient;

        public Task Handle(PlaceOrderV1 message, IMessageHandlerContext context)
        {
            Log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };
            return context.Publish(orderPlaced);
        }

        public async Task Handle(PlaceOrderV2 message, IMessageHandlerContext context)
        {
            Log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");

            // Update CRM.
            var res = await crmHttpClient.GetAsync("https://dibranmulder.github.io/2020/09/22/Improving-your-Azure-Search-performance/");
            if (res.IsSuccessStatusCode)
            {
                Console.WriteLine("CRM Called");
            }

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };
            await context.Publish(orderPlaced);
        }
    }
}