using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shopbridge_base.Data;
using Shopbridge_base.Domain.Models;
using Shopbridge_base.Domain.Services.Interfaces;
using RabbitMQ.Client;
using System.Text;
using Shopbridge_base.Domain.Services.Implementations;

namespace Shopbridge_base.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ILogger<ProductsController> logger;
        private readonly IEventEmitter eventEmitter;
        public ProductsController(IProductService _productService,
            IEventEmitter eventEmitter)
        {
            this.productService = _productService;
            this.eventEmitter = eventEmitter;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return this.Ok(this.productService.Get());
        }


        [HttpGet("{upc}")]
        public async Task<ActionResult<Product>> GetProduct(string upc)
        {
            return this.Ok(this.productService.Get(upc));
        }


        [HttpPut("{upc}")]
        public async Task<IActionResult> PutProduct(string upc, Product product)
        {
            eventEmitter.Publish(product);
            return this.Ok("Updating product..." + product.ProductUpc);
        }


        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            eventEmitter.Publish(product);
            return this.Ok("New product created..." + product.ProductUpc);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return false;
        }

        private void PublishMessage(Product product)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "shopbridgequeue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                string data = Newtonsoft.Json.JsonConvert.SerializeObject(product);
                var body = Encoding.UTF8.GetBytes(data);

                channel.BasicPublish(exchange: "", routingKey: "shopbridgequeue", basicProperties: null, body: body);
            }
        }
    }
}
