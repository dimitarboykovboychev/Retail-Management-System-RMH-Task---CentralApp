using CentralApp.Models;
using CentralApp.Services;
using MassTransit;

namespace CentralApp.Messages
{
    public class ProductCreatedConsumer : IConsumer<ProductCreated>
    {
        private readonly IProductService _productService;

        public ProductCreatedConsumer(IProductService productService)
        {
            _productService = productService;
        }

        public async Task Consume(ConsumeContext<ProductCreated> context)
        {
            var message = context.Message;
            
            await _productService.CreateProductAsync(new Product()
            {
                ProductId = message.Product.ProductId,
                StoreId = message.StoreId,
                Name = message.Product.Name,
                Description = message.Product.Description,
                Price = message.Product.Price,
                MinPrice = message.Product.MinPrice,
                CreatedOn = message.Product.CreatedOn,
                UpdatedOn = message.Product.UpdatedOn
            });
        }
    }
}
