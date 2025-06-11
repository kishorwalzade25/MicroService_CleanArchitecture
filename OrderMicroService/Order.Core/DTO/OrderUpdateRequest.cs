using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.DTO
{
    public class OrderUpdateRequest
    {
        public OrderUpdateRequest() 
        {
            OrderItems =new List<OrderItemUpdateRequest>();
        }
       public int OrderID {  get; set; }
        public int UserID {  get; set; } 
        public DateTime OrderDate {  get; set; }
        public List<OrderItemUpdateRequest> OrderItems {  get; set; }
    }
}
