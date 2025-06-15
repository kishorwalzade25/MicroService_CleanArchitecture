using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.RabbitMQ
{
    public class ProductDeletionMessage
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
    }
}
