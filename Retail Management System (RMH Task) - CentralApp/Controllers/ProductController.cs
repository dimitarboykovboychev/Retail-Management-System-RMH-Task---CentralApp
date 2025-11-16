using CentralApp.Messages;
using CentralApp.Models;
using CentralApp.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace CentralApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IProductService _productService;

        public ProductController(IPublishEndpoint publishEndpoint, IProductService productService)
        {
            _publishEndpoint = publishEndpoint;
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductExtended product)
        {
            await _productService.CreateProductAsync(product);

            await _publishEndpoint.Publish(new CreateProduct(product), context => context.SetRoutingKey($"{product.StoreId}.product")
);
            return Ok(product);
        }

        [HttpGet]
        public async Task<IEnumerable<ProductExtended>> GetProducts()
        {
            return await _productService.GetProductsAsync();
        }
    }
}
