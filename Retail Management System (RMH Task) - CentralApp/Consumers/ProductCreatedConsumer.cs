using CentralApp.Messages;
using CentralApp.Models;
using CentralApp.Services;
using MassTransit;

namespace CentralApp.Consumers
{
    public class ProductCreatedConsumer : IConsumer<ProductCreated>
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductCreatedConsumer> _logger;

        public ProductCreatedConsumer(IProductService productService, ILogger<ProductCreatedConsumer> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductCreated> context)
        {
            var message = context.Message;
            
            await _productService.CreateProductAsync(new ProductExtended()
            {
                ProductId = message.Product.ProductId,
                StoreId = message.StoreID,
                Name = message.Product.Name,
                Description = message.Product.Description,
                Price = message.Product.Price,
                MinPrice = message.Product.MinPrice,
                CreatedOn = message.Product.CreatedOn,
                UpdatedOn = message.Product.UpdatedOn
            });

            _logger.LogInformation($"Product created: {message.Product.ProductId} for Store: {message.StoreID}");

            await Task.CompletedTask;
        }
    }
}
