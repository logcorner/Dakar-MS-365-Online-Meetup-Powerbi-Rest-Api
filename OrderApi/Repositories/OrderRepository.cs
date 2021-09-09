using OrderApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public async Task<List<Order>> GetOrders(string userId)
        {
            string dataPath = Directory.GetCurrentDirectory();
            string fileName = $@"{dataPath}\Data\data.json";
            await using FileStream openStream = File.OpenRead(fileName);
            var orders =
                await JsonSerializer.DeserializeAsync<List<Order>>(openStream);

            return orders.Where(o => o.SalesEmail == userId).ToList();
        }
    }
}