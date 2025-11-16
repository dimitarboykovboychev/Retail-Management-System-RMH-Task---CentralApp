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
        private readonly ILogger<ProductController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger,
                                 IPublishEndpoint publishEndpoint,
                                  IProductService productService)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            await _productService.CreateProductAsync(product);

            // await _publishEndpoint.Publish(); topic for product created can be published here

            return Ok(product);
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productService.GetProductsAsync();
        }
    }
}
