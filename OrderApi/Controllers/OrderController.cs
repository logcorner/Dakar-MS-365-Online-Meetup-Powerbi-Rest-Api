using Microsoft.AspNetCore.Mvc;
using OrderApi.Models;
using OrderApi.Repositories;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace OrderApi.Controllers
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
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var result = await _orderRepository.GetOrders(userId);
            var data = result.Select(x => new OrderDto
            {
                OrderNum = int.Parse(x.OrderNum),
                OrderDate = DateTime.Parse(x.OrderDate),
                CustName = x.CustName,
                CustomerType = x.CustomerType,
                CustState = x.CustState,
                Discount = decimal.Parse(x.Discount),
                OrderTotal = decimal.Parse(x.OrderTotal),
                Quantity = int.Parse(x.Quantity),
                OrderType = x.OrderType,
                
                Price = decimal.Parse(x.Price),
                ProdCategory = x.ProdCategory,
                ProdName = x.ProdName,
                ProdNumber = x.ProdNumber,
                SalesEmail = x.SalesEmail
            });
            return Ok(data);
        }
    }
}