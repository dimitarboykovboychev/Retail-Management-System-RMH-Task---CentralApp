using CentralApp.Services;
using MassTransit;
using MessageContracts;

namespace CentralApp.Consumers
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

            await _productService.DeleteProductAsync(message.ProductID);

            _logger.LogInformation($"Product with ID {message.ProductID} deleted for Store ID {message.StoreID}");

            await Task.CompletedTask;
        }
    }
}
