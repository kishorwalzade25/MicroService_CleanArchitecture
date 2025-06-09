using eCommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace eCommerce.Infrastructure.eCommerceDbContext
{
    public class eCommDbContext:DbContext
    {
        public eCommDbContext(DbContextOptions<eCommDbContext> options):base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers {  get; set; }
    }
}
