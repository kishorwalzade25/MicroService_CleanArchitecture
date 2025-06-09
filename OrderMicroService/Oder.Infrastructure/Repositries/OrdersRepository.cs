using AutoMapper;
using Oder.Infrastructure.OrderDBContext;
using Order.Core.DTO;
using Order.Core.Entities;
using Order.Core.HttpClients;
using Order.Core.RepositriesContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oder.Infrastructure.Repositries
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly UsersMicroserviceClient _usersMicroserviceClient;

        private readonly IMapper _mapper;
        private readonly OrderDbContext _orderDbContext;

        public OrdersRepository(UsersMicroserviceClient usersMicroserviceClient,IMapper mapper, OrderDbContext orderDbContext)
        {
             _usersMicroserviceClient = usersMicroserviceClient;
           
            _mapper = mapper;
            _orderDbContext = orderDbContext;
        }
        public async Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest)
        {
            if (orderAddRequest == null)
            {
                throw new ArgumentNullException(nameof(orderAddRequest));
            }

            UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(orderAddRequest.UserID);
            if (user == null)
            {
                throw new ArgumentException("Invalid User ID");
            }

            return null;

            //OrderDetails orderInput = _mapper.Map<OrderDetails>(orderAddRequest); //Map OrderAddRequest to 'Order' type (it invokes OrderAddRequestToOrderMappingProfile class)

            ////Generate values
            //foreach (OrderItem orderItem in orderInput.OrderItems)
            //{
            //    orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;
            //}
            //orderInput.TotalBill = orderInput.OrderItems.Sum(temp => temp.TotalPrice);


            //await _orderDbContext.Orders.AddAsync(orderInput);
            //await _orderDbContext.SaveChangesAsync();


            //OrderResponse addedOrderResponse = _mapper.Map<OrderResponse>(orderInput); //Map addedOrder ('Order' type) into 'OrderResponse' type (it invokes OrderToOrderResponseMappingProfile).

            //return addedOrderResponse;
       
        }
    }
}
