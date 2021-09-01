using OrderApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrderApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private List<Order> data = new List<Order>
        {
            new Order
            {
                OerderId =1,
                ProductName ="Apple",
                UnitPrice = new decimal( 0.3),
                Quantity = 100,
                CustomerId ="1"
            },
            new Order
            {
                OerderId =2,
                ProductName ="Orange",
                UnitPrice = new decimal( 0.7),
                Quantity = 360,
                CustomerId ="1"
            },
            new Order
            {
                OerderId =3,
                ProductName ="bananas",
                UnitPrice = new decimal( 0.9),
                Quantity = 720,
                CustomerId ="1"
            }
        };

        public IEnumerable<Order> GetOrders(string username)
        {
            return data.Where(d => d.CustomerId == username);
        }

        public bool IsOwnerOfOrder(int orderIdAsGuid, string ownerId)
        {
            return data.Any(i => i.OerderId == orderIdAsGuid && i.CustomerId == ownerId);
        }
    }
}