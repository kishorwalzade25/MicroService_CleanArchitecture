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
        /// <summary>
        /// Retrieves all Orders asynchronously
        /// </summary>
        /// <returns>Returns all orders from the orders collection</returns>
        Task<IEnumerable<OrderDetails>> GetOrders();


        /// <summary>
        /// Retrieves all Orders based on the specified condition asynchronously
        /// </summary>
        /// <param name="filter">The condition to filter orders</param>
        /// <returns>Returning a collection of matching orders</returns>
        //Task<IEnumerable<OrderDetails?>> GetOrdersByCondition(FilterDefinition<Order> filter);


        /// <summary>
        /// Retrieves a single order based on the specified condition asynchronously
        /// </summary>
        /// <param name="filter">The condition to filter Orders</param>
        /// <returns>Returning matching order</returns>
        //Task<OrderDetails?> GetOrderByCondition(FilterDefinition<Order> filter);


        /// <summary>
        /// Adds a new Order into the Orders collection asynchronously
        /// </summary>
        /// <param name="order">The order to be added</param>
        /// <returns>Returnes the added Order object or null if unsuccessful</returns>
        Task<OrderDetails?> AddOrder(OrderDetails order);


        /// <summary>
        /// Updates an existing order asynchronously
        /// </summary>
        /// <param name="order">The order to be added</param>
        /// <returns>Returns the updated order object; or null if not found</returns>
        Task<OrderDetails?> UpdateOrder(OrderDetails order);


        /// <summary>
        /// Deletes the order asynchronously
        /// </summary>
        /// <param name="orderID">The Order ID based on which we need to delete the order</param>
        /// <returns>Returns true if the deletion is successful, false otherwise</returns>
        Task<bool> DeleteOrder(int orderID);

        Task<OrderDetails?> GetOrderById(int orderID);
        Task<List<OrderDetails>?> GetOrderByProductId(int productId);
        Task<List<OrderDetails>?> GetOrderByUserId(int userId);
    }
}
