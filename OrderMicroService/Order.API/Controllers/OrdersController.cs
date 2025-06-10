using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Core.DTO;
using Order.Core.RepositriesContract;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersController(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        //POST api/Orders
        [HttpPost]
        public async Task<IActionResult> AddOrder(OrderAddRequest orderAddRequest)
        {
            if (orderAddRequest == null)
            {
                return BadRequest("Invalid order data");
            }

            UserDTO? orderResponse = await _ordersRepository.AddOrder(orderAddRequest);

            if (orderResponse == null)
            {
                return Problem("Error in adding order");
            }

            return Ok(orderResponse);
        }
    }
}
