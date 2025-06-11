using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Oder.Infrastructure.OrderDBContext;
using Order.Core.DTO;
using Order.Core.Entities;
using Order.Core.HttpClients;
using Order.Core.RepositriesContract;
using Polly;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oder.Infrastructure.Repositries
{
    public class OrdersRepository : IOrdersRepository
    {
      
       
        private readonly OrderDbContext _orders;

        public OrdersRepository(OrderDbContext orderDbContext)
        {
            _orders = orderDbContext;
        }


        public async Task<OrderDetails?> AddOrder(OrderDetails order)
        {
            order.OrderID = 0;
            order._id = order.OrderID;

            foreach (OrderItem orderItem in order.OrderItems)
            {
                orderItem._id = 0;
            }

            await _orders.Orders.AddAsync(order);
            await _orders.SaveChangesAsync();
            return order;
        }


        public async Task<bool> DeleteOrder(int orderID)
        {
           

            OrderDetails? existingOrder = await _orders.Orders.FindAsync(orderID);

            if (existingOrder == null)
            {
                return false;
            }

            var deleteResult =  _orders.Orders.Remove(existingOrder);
           var count= await _orders.SaveChangesAsync();

            if (count > 0) {return true;}
            return false  ;
        }

        public async Task<OrderDetails?> GetOrderById(int orderID) 
        {
            var order = await _orders.Orders.FirstOrDefaultAsync(x => x.OrderID == orderID);
            if(order == null) return null;
            return order;
        }

        public async Task<OrderDetails?> GetOrderBy(int orderID)
        {
            var order = await _orders.Orders.FirstOrDefaultAsync(x => x.OrderID == orderID);
            if (order == null) return null;
            return order;
        }

        public async Task<List<OrderDetails>?> GetOrderByProductId(int productId)
        {
            var order = await _orders.Orders.Include(x=>x.OrderItems.Where(p=>p.ProductID==productId)).ToListAsync();
            if (order == null) return null;
            return order;
        }

        public async Task<List<OrderDetails>?> GetOrderByUserId(int userId)
        {
            var order = await _orders.Orders.Where(x=>x.UserID == userId).ToListAsync();
            if (order == null) return null;
            return order;
        }




        public async Task<IEnumerable<OrderDetails>> GetOrders()
        {
            return await _orders.Orders.ToListAsync();
        }


        //public async Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter)
        //{
        //    return (await _orders.FindAsync(filter)).ToList();
        //}


        public async Task<OrderDetails?> UpdateOrder(OrderDetails order)
        {
            ///FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderID, order.OrderID);

            OrderDetails? existingOrder = await _orders.Orders.FirstOrDefaultAsync(x=> x.OrderID == order.OrderID);

            if (existingOrder == null)
            {
                return null;
            }
            order._id = existingOrder._id;

            _orders.Update(order);
            await _orders.SaveChangesAsync();

            return order;
        }

        

        
    }
}
