using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.DTO
{
    public class OrderResponse
    {
       public int OrderID {  get; set; }
       public int UserID { get; set; }
       public decimal TotalBill {  get; set; }
       public DateTime OrderDate {  get; set; } 
       public List<OrderItemResponse> OrderItems {  get; set; }
    }
}
