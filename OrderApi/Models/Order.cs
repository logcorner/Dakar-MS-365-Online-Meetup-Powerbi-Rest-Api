namespace OrderApi.Models
{
    public class Order
    {
        public int OerderId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string CustomerId { get; set; }
    }
}