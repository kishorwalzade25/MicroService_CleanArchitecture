using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.DTO
{
    public class OrderAddRequest
    {
       public int UserID { get; set; } 
       public DateTime OrderDate {  get; set; }
       public  List<OrderItemAddRequest> OrderItems {  get; set; } = new List<OrderItemAddRequest>(); 
    }
}
