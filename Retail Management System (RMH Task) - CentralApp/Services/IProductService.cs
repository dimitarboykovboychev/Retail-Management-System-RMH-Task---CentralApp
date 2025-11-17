namespace CentralApp.Services
{
    using Models;

    public interface IProductService
    {
        Task<ProductExtended> CreateProductAsync(ProductExtended product);

        Task<IEnumerable<ProductExtended>> GetProductsAsync();

        Task<ProductExtended> GetProductByNameAsync(string name);

        Task<IEnumerable<ProductExtended>> GetProductsByStoreIdAsync(Guid storeId);

        Task DeleteProductAsync(Guid productId);
    }
}
