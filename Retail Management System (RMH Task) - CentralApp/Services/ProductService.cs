using CentralApp.Data;
using CentralApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CentralApp.Services
{
    public class ProductService: IProductService
    {
        private readonly CentralDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(CentralDbContext dbContext, ILogger<ProductService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                if (product == null || !ValidateProduct(product))
                {
                    return null;
                }

                product.ProductId = Guid.NewGuid();
                product.CreatedOn = DateTime.UtcNow;
                product.UpdatedOn = DateTime.UtcNow;

                await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();

                return product;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating product");

                throw;
            }
        }

        public Task DeleteProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByIdAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var products = new List<Product>();

            try
            {
                products = await _dbContext.Products.ToListAsync();

                return products;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");

                throw;
            }
        }

        public Task<IEnumerable<Product>> GetProductsByStoreIdAsync(Guid storeId)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            if (await _dbContext.Products.AnyAsync(p => p.Name == product.Name))
            {
                var existingProduct = await _dbContext.Products.SingleAsync(p => p.Name == product.Name);

                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.MinPrice = product.MinPrice;
                existingProduct.UpdatedOn = DateTime.UtcNow;

                if (!ValidateProduct(existingProduct))
                {
                    return null;
                }

                _dbContext.Products.Update(existingProduct);
                await _dbContext.SaveChangesAsync();

                return existingProduct;
            }

            return null;
        }

        private bool ValidateProduct(Product product)
        {
            if(string.IsNullOrWhiteSpace(product.Name) || product.Price < 0 || product.MinPrice < 0)
            {
                // logging can be added here

                return false;
            }

            if(product.MinPrice > product.Price || product.MinPrice == 0 || product.Price == 0)
            {
                // logging can be added here

                return false;
            }

            if(product.Description != null && product.Description.Length > 500)
            {
                // logging can be added here

                return false;
            }

            if(product.Name.Length > 100)
            {
                // logging can be added here

                return false;
            }


            return true;
        }
    }
}
