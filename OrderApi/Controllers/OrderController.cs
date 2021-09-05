using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Models;
using OrderApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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
        public ActionResult<List<Order>> Index()
        {
            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var result = _orderRepository.GetOrders(username);
            return Ok(result);
        }
    }
}