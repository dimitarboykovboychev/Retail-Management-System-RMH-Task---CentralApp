using CentralApp.Models;
using CentralApp.Services;
using MassTransit;

namespace CentralApp.Messages
{
    public class ProductDeletedConsumer: IConsumer<ProductDeleted>
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductDeletedConsumer> _logger;

        public ProductDeletedConsumer(IProductService productService, ILogger<ProductDeletedConsumer> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductDeleted> context)
        {
            var message = context.Message;
            
            await _productService.DeleteProductAsync(message.ProductId);

            _logger.LogInformation("Product with ID {ProductId} deleted for Store ID {StoreId}", message.ProductId, message.StoreId);

            await Task.CompletedTask;
        }
    }
}
