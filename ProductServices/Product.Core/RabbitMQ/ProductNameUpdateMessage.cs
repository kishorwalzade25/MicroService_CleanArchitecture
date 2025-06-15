using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.RabbitMQ
{
    public class ProductNameUpdateMessage
    {
        public int ProductID { get; set; }
        public string? NewName {  get; set; }
    }
}
