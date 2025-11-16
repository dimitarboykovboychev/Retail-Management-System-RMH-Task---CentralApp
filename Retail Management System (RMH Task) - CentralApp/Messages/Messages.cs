using CentralApp.Models;

namespace CentralApp.Messages
{
    public record ProductCreated(Guid StoreId, Product Product);

    public record CreateProduct(ProductExtended product);

    public static class MessageQueues
    {
        public const string ProductCreatedQueue = "product-created-queue";
    }
}
