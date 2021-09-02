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
                CustomerEmail ="leyegora@yahoo.fr"
            },
            new Order
            {
                OerderId =2,
                ProductName ="Orange",
                UnitPrice = new decimal( 0.7),
                Quantity = 360,
                CustomerEmail ="leyegora@yahoo.fr"
            },
            new Order
            {
                OerderId =3,
                ProductName ="bananas",
                UnitPrice = new decimal( 0.9),
                Quantity = 720,
                CustomerEmail ="leyegora@yahoo.fr"
            }
        };

        public IEnumerable<Order> GetOrders(string username)
        {
            return data.Where(d => d.CustomerEmail == username);
        }

        public bool IsOwnerOfOrder(int orderIdAsGuid, string ownerId)
        {
            return data.Any(i => i.OerderId == orderIdAsGuid && i.CustomerEmail == ownerId);
        }
    }
}