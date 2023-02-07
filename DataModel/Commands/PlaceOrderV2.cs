using DataModel.Models;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Commands
{
    public class PlaceOrderV2
    {
        public string OrderId { get; set; }
        public Product Product { get; set; }
        public string Buyer { get; set; }
    }
}
