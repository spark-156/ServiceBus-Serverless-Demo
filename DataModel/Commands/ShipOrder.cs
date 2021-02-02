using NServiceBus;

namespace DataModel.Commands
{
    public class ShipOrder : ICommand
    {
        public string OrderId { get; set; }
    }
}