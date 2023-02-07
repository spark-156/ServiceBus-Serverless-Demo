using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using DataModel.Commands;
using DataModel.Events;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SalesFunctions;

public static class PlacedOrderHandler
{
    [FunctionName("PlacedOrderHandler")]
    [return: ServiceBus("ordersplaced", Connection = "ServiceBusConnection")]
    public static string Run([ServiceBusTrigger("placeorder", "Sales", Connection = "ServiceBusConnection")] string message, Int32 deliveryCount,
        DateTime enqueuedTimeUtc,
        string messageId,
        ILogger log)
    {
        log.LogInformation($"C# ServiceBus queue trigger function processed message: {message}");
        log.LogInformation($"EnqueuedTimeUtc={enqueuedTimeUtc}");
        log.LogInformation($"DeliveryCount={deliveryCount}");
        log.LogInformation($"MessageId={messageId}");
        
        PlaceOrderV1 order = JsonConvert.DeserializeObject<PlaceOrderV1>(message);

        OrderPlaced orderPlaced = new OrderPlaced()
        {
            OrderId = order.OrderId
        };
        
        return JsonConvert.SerializeObject(orderPlaced);
    }
}