using Order.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.RepositriesContract
{
    public interface IOrdersService
    {
        Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest);
    }
}
