using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.DTO
{
    
        public class UserDTO
        {
            public int UserID { get; set; }
            public string? Email { get; set; }
            public string? PersonName { get; set; }
            public string? Gender { get; set; }
        }
    
}
