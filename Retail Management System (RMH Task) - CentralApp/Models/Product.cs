namespace CentralApp.Models
{
    public class Product
    {
        public Guid ProductID { get; set; }

        public Guid StoreID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal MinPrice { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
