using DataModel.Commands;
using DataModel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly Product[] Products = new[]
        {
            new Product() { Name = "Book", Price = 9.99, Manufacturer = "O'Reilly" },
            new Product() { Name = "Car", Price = 45000, Manufacturer = "Tesla" },
            new Product() { Name = "Starship", Price = 9999999999, Manufacturer = "SpaceX" },
        };

        private readonly ILogger<ProductsController> _logger;
        private readonly IMessageSession messageSession;

        public ProductsController(ILogger<ProductsController> logger, IMessageSession messageSession)
        {
            _logger = logger;
            this.messageSession = messageSession;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return Products;
        }

        [HttpPost("v1/order")]
        public async Task<IActionResult> OrderProductAsync(string productName)
        {
            var product = Products.FirstOrDefault(x => x.Name == productName);
            if (product is null)
            {
                return NotFound();
            }

            var command = new PlaceOrderV1
            {
                OrderId = Guid.NewGuid().ToString(),
                Product = product
            };

            await messageSession.Send(command);

            return Ok();
        }

        [HttpPost("v2/order")]
        public async Task<IActionResult> OrderProductAsync(string productName, string buyer)
        {
            var product = Products.FirstOrDefault(x => x.Name == productName);
            if (product is null)
            {
                return NotFound();
            }

            var command = new PlaceOrderV2
            {
                OrderId = Guid.NewGuid().ToString(),
                Product = product,
                Buyer = buyer
            };

            await messageSession.Send(command);

            return Ok();
        }
    }
}
