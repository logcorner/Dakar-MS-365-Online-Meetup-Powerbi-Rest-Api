using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Repositories;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace web_api_core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            var result = await _orderRepository.GetOrders(userId);

            return Ok(result);
        }
    }
}