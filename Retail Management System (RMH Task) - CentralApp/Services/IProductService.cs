namespace CentralApp.Services
{
    using Models;

    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);

        Task<IEnumerable<Product>> GetProductsAsync();

        Task<Product> GetProductByNameAsync(string name, Guid storeID);

        Task<IEnumerable<Product>> GetProductsByStoreIDAsync(Guid storeID);

        Task DeleteProductAsync(Guid productID);
    }
}
