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
                if (product == null || !this.ValidateProduct(product))
                {
                    return null;
                }

                if(await _dbContext.Products.AnyAsync(p => p.Name == product.Name && p.StoreID == product.StoreID))
                {
                    return await this.UpdateProductAsync(product);
                }

                product.ProductID = product.ProductID == Guid.Empty ? Guid.NewGuid() : product.ProductID;
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

        public async Task DeleteProductAsync(Guid productID)
        {
            if (_dbContext.Products.Any(p => p.ProductID == productID))
            {
                var product = _dbContext.Products.Single(p => p.ProductID == productID);

                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Product> GetProductByNameAsync(string name, Guid storeID)
        {
            return await _dbContext.Products.SingleOrDefaultAsync(p => p.Name == name && p.StoreID == storeID);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            try
            {
                var products = await _dbContext.Products.ToListAsync();

                return products;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");

                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByStoreIDAsync(Guid storeId)
        {
            return await _dbContext.Products.Where(p => p.StoreID == storeId).ToListAsync();
        }

        private async Task<Product> UpdateProductAsync(Product product)
        {
            var existingProduct = await _dbContext.Products.SingleAsync(p => p.Name == product.Name && p.StoreID == product.StoreID);

            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.MinPrice = product.MinPrice;
            existingProduct.UpdatedOn = DateTime.UtcNow;

            if (!this.ValidateProduct(existingProduct))
            {
                return null;
            }

            _dbContext.Products.Update(existingProduct);
            await _dbContext.SaveChangesAsync();

            return existingProduct;
        }

        private bool ValidateProduct(Product product)
        {
            if(string.IsNullOrWhiteSpace(product.Name) || product.Price < 0 || product.MinPrice < 0 || product.Price == 0 || product.MinPrice == 0)
            {
                _logger.LogWarning($"Product {product.ProductID} has invalid Name, Price or MinPrice.", product.ProductID);

                return false;
            }

            if(product.MinPrice > product.Price)
            {
                _logger.LogWarning($"Product {product.ProductID} has a MinPrice greater than its Price.", product.ProductID);

                return false;
            }

            if(product.Description != null && product.Description.Length > 500)
            {
                _logger.LogWarning($"Product {product.ProductID} has a Description longer than 500 characters.", product.ProductID);

                return false;
            }

            if(product.Name.Length > 100)
            {
                _logger.LogWarning($"Product {product.ProductID} has a Name longer than 100 characters.", product.ProductID);

                return false;
            }

            return true;
        }
    }
}
