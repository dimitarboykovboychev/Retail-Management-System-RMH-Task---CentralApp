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

        public async Task<ProductExtended> CreateProductAsync(ProductExtended product)
        {
            try
            {
                if (product == null || !ValidateProduct(product))
                {
                    return null;
                }

                if(await _dbContext.Products.AnyAsync(p => p.Name == product.Name))
                {
                    return await UpdateProductAsync(product);
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

        public async Task DeleteProductAsync(Guid productId)
        {
            if (_dbContext.Products.Any(p => p.ProductId == productId))
            {
                var product = _dbContext.Products.Single(p => p.ProductId == productId);

                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<ProductExtended> GetProductByNameAsync(string name)
        {
            return await _dbContext.Products.SingleOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<ProductExtended>> GetProductsAsync()
        {
            var products = new List<ProductExtended>();

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

        public async Task<IEnumerable<ProductExtended>> GetProductsByStoreIdAsync(Guid storeId)
        {
            return await _dbContext.Products.Where(p => p.StoreId == storeId).ToListAsync();
        }

        private async Task<ProductExtended> UpdateProductAsync(ProductExtended product)
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

        private bool ValidateProduct(ProductExtended product)
        {
            if(string.IsNullOrWhiteSpace(product.Name) || product.Price < 0 || product.MinPrice < 0 || product.Price == 0 || product.MinPrice == 0)
            {
                _logger.LogWarning("Product {ProductId} has invalid Name, Price or MinPrice.", product.ProductId);

                return false;
            }

            if(product.MinPrice > product.Price)
            {
                _logger.LogWarning("Product {ProductId} has a MinPrice greater than its Price.", product.ProductId);

                return false;
            }

            if(product.Description != null && product.Description.Length > 500)
            {
                _logger.LogWarning("Product {ProductId} has a Description longer than 500 characters.", product.ProductId);

                return false;
            }

            if(product.Name.Length > 100)
            {
                _logger.LogWarning("Product {ProductId} has a Name longer than 100 characters.", product.ProductId);

                return false;
            }

            return true;
        }
    }
}
