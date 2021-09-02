namespace OrderApi.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string CustomerEmail { get; set; }
    }
}