using CentralApp.Models;

namespace CentralApp.Messages
{
    public record ProductCreated(Guid StoreID, Product Product);

    public record ProductDeleted(Guid StoreID, Guid ProductId);


    public record CreateProduct(ProductExtended ProductExtended);

    public record DeleteProduct(Guid StoreID, Guid ProductId);
}
