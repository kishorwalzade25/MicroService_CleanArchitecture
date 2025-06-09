using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO
{
    public class AuthenticationResponse
    {
        [Key]
        public int UserID { get; set; }
        public string? Email { get; set; }
        public string? PersonName { get; set; }
        public string? Gender { get; set; }
        public string? Token { get; set; }
    }
}
