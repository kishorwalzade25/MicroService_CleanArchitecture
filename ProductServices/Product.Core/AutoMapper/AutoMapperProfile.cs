using AutoMapper;
using Product.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<ProductAddRequest, ProductItem>().ReverseMap();
            CreateMap<ProductResponse, ProductItem>().ReverseMap();
        }
    }
}
