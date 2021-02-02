using NServiceBus;

namespace DataModel.Events
{
    public class OrderPlaced :IEvent
    {
        public string OrderId { get; set; }
    }
}