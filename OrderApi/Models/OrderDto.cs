using System;

namespace OrderApi.Models
{
    public class OrderDto
    {
        public int OrderNum { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderType { get; set; }
        public string CustomerType { get; set; }
        public string CustName { get; set; }
        public string CustState { get; set; }
        public string ProdCategory { get; set; }
        public string ProdNumber { get; set; }
        public string ProdName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal OrderTotal { get; set; }
        public string SalesEmail { get; set; }
    }
}