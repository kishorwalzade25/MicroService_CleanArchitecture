using Microsoft.EntityFrameworkCore;
using Order.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oder.Infrastructure.OrderDBContext
{
    public class OrderDbContext:DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options):base(options) { }

        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderDetails> Orders { get; set; }
    }
}
