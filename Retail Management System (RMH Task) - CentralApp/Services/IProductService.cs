namespace CentralApp.Services
{
    using Models;

    public interface IProductService
    {
        Task<ProductExtended> CreateProductAsync(ProductExtended product);

        Task<IEnumerable<ProductExtended>> GetProductsAsync();

        Task<ProductExtended> GetProductByIdAsync(Guid productId);

        Task<ProductExtended> GetProductByNameAsync(string name);

        Task<IEnumerable<ProductExtended>> GetProductsByStoreIdAsync(Guid storeId);

        Task<ProductExtended> UpdateProductAsync(ProductExtended product);

        Task DeleteProductAsync(ProductExtended product);
    }
}
