namespace CentralApp.Services
{
    using Models;

    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);

        Task<IEnumerable<Product>> GetProductsAsync();

        Task<Product> GetProductByIdAsync(Guid productId);

        Task<Product> GetProductByNameAsync(string name);

        Task<IEnumerable<Product>> GetProductsByStoreIdAsync(Guid storeId);

        Task<Product> UpdateProductAsync(Product product);

        Task DeleteProductAsync(Product product);
    }
}
