using CentralApp.Models;

namespace CentralApp.Messages
{
    public record ProductCreated(Guid StoreId, Product Product);

    public record ProductDeleted(Guid StoreId, Guid ProductId);

    public record CreateProduct(ProductExtended Product);

    public record DeleteProduct(Guid StoreId, Guid ProductId);

    public static class MessageQueues
    {
        public const string ProductQueue = "product-queue";
    }
}
