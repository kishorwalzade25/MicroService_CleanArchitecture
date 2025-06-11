using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.Entities
{
    public class ProductAddRequest
    {
            public string? ProductName { get; set; }
            public string? Category { get; set; }
            public double? UnitPrice { get; set; }
            public int? QuantityInStock { get; set; }
    }
}
