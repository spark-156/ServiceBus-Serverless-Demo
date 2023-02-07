using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using DataModel.Commands;
using DataModel.Events;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SalesFunctions;

public static class PlacedOrderV2Handler
{
    private static CRMHttpClient _crmHttpClient = new CRMHttpClient();

    [FunctionName("PlacedOrderV2Handler")]
    [return: ServiceBus("ordersplaced", Connection = "ServiceBusConnection")]
    public static async Task<string> AsyncRun([ServiceBusTrigger("placeorderv2", "Sales", Connection = "ServiceBusConnection")] string message, Int32 deliveryCount,
        DateTime enqueuedTimeUtc,
        string messageId,
        ILogger log)
    {
        log.LogInformation($"C# ServiceBus queue trigger function processed message: {message}");
        log.LogInformation($"EnqueuedTimeUtc={enqueuedTimeUtc}");
        log.LogInformation($"DeliveryCount={deliveryCount}");
        log.LogInformation($"MessageId={messageId}");
        
        PlaceOrderV2 order = JsonConvert.DeserializeObject<PlaceOrderV2>(message);
        
        var res = await _crmHttpClient.GetAsync(
            "https://dibranmulder.github.io/2020/09/22/Improving-your-Azure-Search-performance/");
        if (res.IsSuccessStatusCode)
        {
            Console.WriteLine("CRM Called");
        }

        OrderPlaced orderPlaced = new OrderPlaced()
        {
            OrderId = order.OrderId
        };
        return JsonConvert.SerializeObject(orderPlaced);
    }
}
