using NServiceBus;

namespace DataModel.Events
{
    public class OrderBilled : IEvent
    {
        public string OrderId { get; set; }
    }
}