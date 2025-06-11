using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oder.Infrastructure.Repositries;
using Order.Core.DTO;
using Order.Core.Entities;
using Order.Core.RepositriesContract;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersRepository)
        {
            _ordersService = ordersRepository;
        }

        //GET: /api/Orders
        [HttpGet]
        public async Task<IEnumerable<OrderResponse?>> Get()
        {
            List<OrderResponse?> orders = await _ordersService.GetOrders();
            return orders;
        }


        //GET: /api/Orders/search/orderid/{orderID}
        [HttpGet("search/orderid/{orderID}")]
        public async Task<OrderResponse?> GetOrderByOrderID(int orderID)
        {
           

            OrderResponse? order = await _ordersService.GetOrderById(orderID);
            return order;
        }


        ////GET: /api/Orders/search/productid/{productID}
        [HttpGet("search/productid/{productID}")]
        public async Task<IEnumerable<OrderResponse>?> GetOrdersByProductID(int productID)
        {


            List<OrderResponse>? orders = await _ordersService.GetOrderByProductId(productID);
            return orders;
        }


        ////GET: /api/Orders/search/orderDate/{orderDate}
        //[HttpGet("search/orderDate/{orderDate}")]
        //public async Task<IEnumerable<OrderResponse?>> GetOrdersByOrderDate(DateTime orderDate)
        //{
        //    FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderDate.ToString("yyyy-MM-dd"), orderDate.ToString("yyyy-MM-dd")
        //      );

        //    List<OrderResponse?> orders = await _ordersRepository.GetOrdersByCondition(filter);
        //    return orders;
        //}


        ////GET: /api/Orders/search/userid/{userID}
        [HttpGet("search/userid/{userID}")]
        public async Task<IEnumerable<OrderResponse>?> GetOrdersByUserID(int userID)
        {


            List<OrderResponse>? orders = await _ordersService.GetOrderByUserId(userID);
            return orders;
        }


        //POST api/Orders
        [HttpPost]
        public async Task<IActionResult> Post(OrderAddRequest orderAddRequest)
        {
            if (orderAddRequest == null)
            {
                return BadRequest("Invalid order data");
            }

            OrderResponse? orderResponse = await _ordersService.AddOrder(orderAddRequest);

            if (orderResponse == null)
            {
                return Problem("Error in adding order");
            }


            return Created($"api/Orders/search/orderid/{orderResponse?.OrderID}", orderResponse);
        }


        //PUT api/Orders/{orderID}
        [HttpPut("{orderID}")]
        public async Task<IActionResult> Put(int orderID, OrderUpdateRequest orderUpdateRequest)
        {
            if (orderUpdateRequest == null)
            {
                return BadRequest("Invalid order data");
            }

            if (orderID != orderUpdateRequest.OrderID)
            {
                return BadRequest("OrderID in the URL doesn't match with the OrderID in the Request body");
            }

            OrderResponse? orderResponse = await _ordersService.UpdateOrder(orderUpdateRequest);

            if (orderResponse == null)
            {
                return Problem("Error in updating order");
            }


            return Ok(orderResponse);
        }


        //DELETE api/Orders/{orderID}
        [HttpDelete("{orderID}")]
        public async Task<IActionResult> Delete(int orderID)
        {
            if (orderID == 0)
            {
                return BadRequest("Invalid order ID");
            }

            bool isDeleted = await _ordersService.DeleteOrder(orderID);

            if (!isDeleted)
            {
                return Problem("Error in deleting order");
            }

            return Ok(isDeleted);
        }
    }
}
