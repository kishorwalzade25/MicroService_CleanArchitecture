using Order.Core.DTO;
using Order.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.RepositriesContract
{
    public interface IOrdersRepository
    {
       // Task<IEnumerable<OrderDetails>> GetOrders();


        
        //Task<IEnumerable<OrderDetails?>> GetOrdersByCondition(FilterDefinition<OrderDetails> filter); 
        //Task<OrderDetails?> GetOrderByCondition(FilterDefinition<OrderDetails> filter);

        Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest);
        //Task<OrderDetails?> UpdateOrder(OrderDetails order);

        //Task<bool> DeleteOrder(Guid orderID);
    }
}
