using AutoMapper;
using Order.Core.DTO;
using Order.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.AutoMapper
{
    public class OrderAutoMapper:Profile
    {
        public OrderAutoMapper() 
        {
            CreateMap<OrderAddRequest,OrderDetails>().ReverseMap();
            CreateMap<OrderResponse, OrderDetails>().ReverseMap();
        }
    }
}
