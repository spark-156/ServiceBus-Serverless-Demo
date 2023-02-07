using DataModel.Models;
using NServiceBus;

namespace DataModel.Commands
{
    public class PlaceOrderV1
    {
        public string OrderId { get; set; }
        public Product Product { get; set; }
    }
}