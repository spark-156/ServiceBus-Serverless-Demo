using System.Threading.Tasks;
using DataModel.Events;
using NServiceBus;
using NServiceBus.Logging;

namespace BillingFunctions
{
    public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
    {
        static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

        public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced, OrderId = {message.OrderId} - Charging credit card...");

            await context.Publish(new OrderBilled
            {
                OrderId = message.OrderId
            });
        }
    }
}