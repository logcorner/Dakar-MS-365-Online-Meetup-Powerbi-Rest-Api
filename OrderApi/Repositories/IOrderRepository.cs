using OrderApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderApi.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrders();
    }
}