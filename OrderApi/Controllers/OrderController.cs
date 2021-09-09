using Microsoft.AspNetCore.Mvc;
using OrderApi.Models;
using OrderApi.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        //[Authorize]
        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            var result = await _orderRepository.GetOrders();
            var data = result.Select(x => new OrderDto
            {
                OrderNum = int.Parse(x.OrderNum),
                OrderDate = DateTime.Parse(x.OrderDate),
                CustName = x.CustName,
                CustomerType = x.CustomerType,
                CustState = x.CustState,
                Discount = decimal.Parse(x.Discount),
                OrderTotal = decimal.Parse(x.OrderTotal),
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