using DataModel.Commands;
using DataModel.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace RestAPI_2._0.Controllers;

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

    private readonly MessageSender _messageSender = new MessageSender();

    public ProductsController()
    {
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
        
        Console.WriteLine(command.Product);

        await _messageSender.Send(command);
        
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

        await _messageSender.Send(command);
        
        return Ok();
    }
}