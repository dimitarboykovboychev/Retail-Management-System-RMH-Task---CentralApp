using CentralApp.Models;

namespace CentralApp.Messages
{
    public record ProductCreated(Guid StoreId, Product Product);

    public static class MessageQueues
    {
        public const string ProductCreatedQueue = "product-created-queue";
    }
}
