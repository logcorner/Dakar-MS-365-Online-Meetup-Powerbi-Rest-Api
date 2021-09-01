using OrderApi.Models;
using System.Collections.Generic;

namespace OrderApi.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders(string username);

        bool IsOwnerOfOrder(int orderId, string ownerId);
    }
}