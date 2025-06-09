using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.DTO
{
    public class OrderItemAddRequest
    {
        public int ProductID { get; set; }
        public decimal UnitPrice {  get; set; } 
        public int Quantity {  get; set; }
    }
}
