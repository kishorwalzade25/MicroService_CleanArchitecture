using AutoMapper;
using Order.Core.DTO;
using Order.Core.Entities;
using Order.Core.HttpClients;
using Order.Core.RepositriesContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oder.Infrastructure.Repositries
{
    public class OrdersService:IOrdersService
    {
        
        private readonly IMapper _mapper;
        private IOrdersRepository _ordersRepository;
        private UsersMicroserviceClient _usersMicroserviceClient;
        private ProductsMicroserviceClient _productsMicroserviceClient;


        public OrdersService(IOrdersRepository ordersRepository, IMapper mapper, UsersMicroserviceClient usersMicroserviceClient, ProductsMicroserviceClient productsMicroserviceClient)
        {
            
            _mapper = mapper;
            _ordersRepository = ordersRepository;
            _usersMicroserviceClient = usersMicroserviceClient;
            _productsMicroserviceClient = productsMicroserviceClient;
        }


        public async Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest)
        {
            //Check for null parameter
            if (orderAddRequest == null)
            {
                throw new ArgumentNullException(nameof(orderAddRequest));
            }


            

            List<ProductDTO?> products = new List<ProductDTO?>();

            //Validate order items using Fluent Validation
            foreach (OrderItemAddRequest orderItemAddRequest in orderAddRequest.OrderItems)
            {
                


                //TO DO: Add logic for checking if ProductID exists in Products microservice
                ProductDTO? product = await _productsMicroserviceClient.GetProductByProductID(orderItemAddRequest.ProductID);
                if (product == null)
                {
                    throw new ArgumentException("Invalid Product ID");
                }

                products.Add(product);
            }

            //TO DO: Add logic for checking if UserID exists in Users microservice
            UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(orderAddRequest.UserID);
            if (user == null)
            {
                throw new ArgumentException("Invalid User ID");
            }


            //Convert data from OrderAddRequest to Order
            OrderDetails orderInput = _mapper.Map<OrderDetails>(orderAddRequest); //Map OrderAddRequest to 'Order' type (it invokes OrderAddRequestToOrderMappingProfile class)

            //Generate values
            foreach (OrderItem orderItem in orderInput.OrderItems)
            {
                orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;
            }
            orderInput.TotalBill = orderInput.OrderItems.Sum(temp => temp.TotalPrice);


            //Invoke repository
            OrderDetails? addedOrder = await _ordersRepository.AddOrder(orderInput);

            if (addedOrder == null)
            {
                return null;
            }

            OrderResponse addedOrderResponse = _mapper.Map<OrderResponse>(addedOrder); //Map addedOrder ('Order' type) into 'OrderResponse' type (it invokes OrderToOrderResponseMappingProfile).

            //TO DO: Load ProductName and Category in OrderItem
            if (addedOrderResponse != null)
            {
                foreach (OrderItemResponse orderItemResponse in addedOrderResponse.OrderItems)
                {
                    ProductDTO? productDTO = products.Where(temp => temp.ProductID == orderItemResponse.ProductID).FirstOrDefault();

                    if (productDTO == null)
                        continue;

                    _mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItemResponse);
                }
            }



            //TO DO: Load UserPersonName and Email from Users Microservice
            if (addedOrderResponse != null)
            {
                if (user != null)
                {
                    _mapper.Map<UserDTO, OrderResponse>(user, addedOrderResponse);
                }
            }

            return addedOrderResponse;
        }



        public async Task<OrderResponse?> UpdateOrder(OrderUpdateRequest orderUpdateRequest)
        {
            //Check for null parameter
            if (orderUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(orderUpdateRequest));
            }


            

            List<ProductDTO> products = new List<ProductDTO>();

            //Validate order items using Fluent Validation
            foreach (OrderItemUpdateRequest orderItemUpdateRequest in orderUpdateRequest.OrderItems)
            {
                

                //TO DO: Add logic for checking if ProductID exists in Products microservice
                ProductDTO? product = await _productsMicroserviceClient.GetProductByProductID(orderItemUpdateRequest.ProductID);
                if (product == null)
                {
                    throw new ArgumentException("Invalid Product ID");
                }

                products.Add(product);
            }

            //TO DO: Add logic for checking if UserID exists in Users microservice
            UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(orderUpdateRequest.UserID);
            if (user == null)
            {
                throw new ArgumentException("Invalid User ID");
            }


            //Convert data from OrderUpdateRequest to Order
            OrderDetails orderInput = _mapper.Map<OrderDetails>(orderUpdateRequest); //Map OrderUpdateRequest to 'Order' type (it invokes OrderUpdateRequestToOrderMappingProfile class)

            //Generate values
            foreach (OrderItem orderItem in orderInput.OrderItems)
            {
                orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;
            }
            orderInput.TotalBill = orderInput.OrderItems.Sum(temp => temp.TotalPrice);


            //Invoke repository
            OrderDetails? updatedOrder = await _ordersRepository.UpdateOrder(orderInput);

            if (updatedOrder == null)
            {
                return null;
            }

            OrderResponse updatedOrderResponse = _mapper.Map<OrderResponse>(updatedOrder); //Map updatedOrder ('Order' type) into 'OrderResponse' type (it invokes OrderToOrderResponseMappingProfile).


            //TO DO: Load ProductName and Category in OrderItem
            if (updatedOrderResponse != null)
            {
                foreach (OrderItemResponse orderItemResponse in updatedOrderResponse.OrderItems)
                {
                    ProductDTO? productDTO = products.Where(temp => temp.ProductID == orderItemResponse.ProductID).FirstOrDefault();

                    if (productDTO == null)
                        continue;

                    _mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItemResponse);
                }
            }


            //TO DO: Load UserPersonName and Email from Users Microservice
            if (updatedOrderResponse != null)
            {
                if (user != null)
                {
                    _mapper.Map<UserDTO, OrderResponse>(user, updatedOrderResponse);
                }
            }

            return updatedOrderResponse;
        }


        public async Task<bool> DeleteOrder(int orderID)
        {
            //FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderID, orderID);
            bool existingOrder = await _ordersRepository.DeleteOrder(orderID);

            if (existingOrder == false)
            {
                return false;
            }


            
            return existingOrder;
        }


        //public async Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter)
        //{
        //    Order? order = await _ordersRepository.GetOrderByCondition(filter);
        //    if (order == null)
        //        return null;

        //    OrderResponse orderResponse = _mapper.Map<OrderResponse>(order);


        //    //TO DO: Load ProductName and Category in OrderItem
        //    if (orderResponse != null)
        //    {
        //        foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
        //        {
        //            ProductDTO? productDTO = await _productsMicroserviceClient.GetProductByProductID(orderItemResponse.ProductID);

        //            if (productDTO == null)
        //                continue;

        //            _mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItemResponse);
        //        }
        //    }


        //    //TO DO: Load UserPersonName and Email from Users Microservice
        //    if (orderResponse != null)
        //    {
        //        UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(orderResponse.UserID);
        //        if (user != null)
        //        {
        //            _mapper.Map<UserDTO, OrderResponse>(user, orderResponse);
        //        }
        //    }

        //    return orderResponse;
        //}


        //public async Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter)
        //{
        //    IEnumerable<Order?> orders = await _ordersRepository.GetOrdersByCondition(filter);


        //    IEnumerable<OrderResponse?> orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);


        //    //TO DO: Load ProductName and Category in each OrderItem
        //    foreach (OrderResponse? orderResponse in orderResponses)
        //    {
        //        if (orderResponse == null)
        //        {
        //            continue;
        //        }

        //        foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
        //        {
        //            ProductDTO? productDTO = await _productsMicroserviceClient.GetProductByProductID(orderItemResponse.ProductID);

        //            if (productDTO == null)
        //                continue;

        //            _mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItemResponse);
        //        }


        //        //TO DO: Load UserPersonName and Email from Users Microservice
        //        UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(orderResponse.UserID);
        //        if (user != null)
        //        {
        //            _mapper.Map<UserDTO, OrderResponse>(user, orderResponse);
        //        }
        //    }

        //    return orderResponses.ToList();
        //}


        public async Task<List<OrderResponse?>> GetOrders()
        {
            IEnumerable<OrderDetails?> orders = await _ordersRepository.GetOrders();

            IEnumerable<OrderResponse?> orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);


            //TO DO: Load ProductName and Category in each OrderItem
            foreach (OrderResponse? orderResponse in orderResponses)
            {
                if (orderResponse == null)
                {
                    continue;
                }

                foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
                {
                    ProductDTO? productDTO = await _productsMicroserviceClient.GetProductByProductID(orderItemResponse.ProductID);

                    if (productDTO == null)
                        continue;

                    _mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItemResponse);
                }


                //TO DO: Load UserPersonName and Email from Users Microservice
                UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(orderResponse.UserID);
                if (user != null)
                {
                    _mapper.Map<UserDTO, OrderResponse>(user, orderResponse);
                }
            }


            return orderResponses.ToList();
        }

        public async Task<OrderResponse?> GetOrderById(int orderID)
        {
            OrderDetails? order = await _ordersRepository.GetOrderById(orderID);
            if (order == null)
                return null;

            OrderResponse orderResponse = _mapper.Map<OrderResponse>(order);


            //TO DO: Load ProductName and Category in OrderItem
            if (orderResponse != null)
            {
                foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
                {
                    ProductDTO? productDTO = await _productsMicroserviceClient.GetProductByProductID(orderItemResponse.ProductID);

                    if (productDTO == null)
                        continue;

                    _mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItemResponse);
                }
            }


            //TO DO: Load UserPersonName and Email from Users Microservice
            if (orderResponse != null)
            {
                UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(orderResponse.UserID);
                if (user != null)
                {
                    _mapper.Map<UserDTO, OrderResponse>(user, orderResponse);
                }
            }

            return orderResponse;
        }

        public async Task<List<OrderResponse>?> GetOrderByProductId(int productId)
        {
            IEnumerable<OrderDetails>? orders = await _ordersRepository.GetOrderByProductId(productId);


            IEnumerable<OrderResponse>? orderResponses = _mapper.Map<IEnumerable<OrderResponse>?>(orders);

            if (orderResponses != null)
            {
                //TO DO: Load ProductName and Category in each OrderItem
                foreach (OrderResponse? orderResponse in orderResponses)
                {
                    if (orderResponse == null)
                    {
                        continue;
                    }

                    foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
                    {
                        ProductDTO? productDTO = await _productsMicroserviceClient.GetProductByProductID(orderItemResponse.ProductID);

                        if (productDTO == null)
                            continue;

                        _mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItemResponse);
                    }


                    //TO DO: Load UserPersonName and Email from Users Microservice
                    UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(orderResponse.UserID);
                    if (user != null)
                    {
                        _mapper.Map<UserDTO, OrderResponse>(user, orderResponse);
                    }
                }
                return orderResponses.ToList();
            }
            return null;
        }

        public async Task<List<OrderResponse>?> GetOrderByUserId(int userId)
        {

            IEnumerable<OrderDetails>? orders = await _ordersRepository.GetOrderByUserId(userId);


            IEnumerable<OrderResponse>? orderResponses = _mapper.Map<IEnumerable<OrderResponse>?>(orders);

            if (orderResponses != null)
            {
                //TO DO: Load ProductName and Category in each OrderItem
                foreach (OrderResponse? orderResponse in orderResponses)
                {
                    if (orderResponse == null)
                    {
                        continue;
                    }

                    foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
                    {
                        ProductDTO? productDTO = await _productsMicroserviceClient.GetProductByProductID(orderItemResponse.ProductID);

                        if (productDTO == null)
                            continue;

                        _mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItemResponse);
                    }


                    //TO DO: Load UserPersonName and Email from Users Microservice
                    UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(orderResponse.UserID);
                    if (user != null)
                    {
                        _mapper.Map<UserDTO, OrderResponse>(user, orderResponse);
                    }
                }
                return orderResponses.ToList();
            }
            return null;
        }
    }
}
