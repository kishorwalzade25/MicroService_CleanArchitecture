using Microsoft.EntityFrameworkCore;
using Product.Core.Entities;


namespace Product.Infrastructure.ProductContextDB
{
    public class ProductDbContext:DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options):base(options) { }

        public DbSet<ProductItem> Products { get; set; }
    }
}
