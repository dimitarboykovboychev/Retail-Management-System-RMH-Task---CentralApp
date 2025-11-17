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

            var storeID = "35ae9dcb-7ba0-4929-baea-d339b5e4523a"; // Example store ID for routing key

            await _publishEndpoint.Publish(new CreateProduct(product), context => context.SetRoutingKey($"{storeID/*product.StoreId*/}.product")
);
            return Ok(product);
        }

        [HttpGet]
        public async Task<IEnumerable<ProductExtended>> GetProducts()
        {
            return await _productService.GetProductsAsync();
        }

        [HttpDelete("{storeId}/{productId}")]
        public async Task<IActionResult> Delete(Guid storeId, Guid productId)
        {
            await _productService.DeleteProductAsync(productId);

            await _publishEndpoint.Publish(new DeleteProduct(storeId, productId), context => context.SetRoutingKey($"{storeId}.product"));
            
            return NoContent();
        }
    }
}
