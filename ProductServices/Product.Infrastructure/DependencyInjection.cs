using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Core.RepositriesService;
using Product.Infrastructure.ProductContextDB;
using Product.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
        {
            var ConnTemplate = configuration.GetConnectionString("productService")!;

            //      string connectionString = ConnTemplate
            //.Replace("$Product_db", Environment.GetEnvironmentVariable("MYSQL_HOST"))
            //.Replace("$Product_Password", Environment.GetEnvironmentVariable("MYSQL_PASSWORD"));

            string connectionString = ConnTemplate
      .Replace("$Product_db", "localhost")
      .Replace("$Product_Password", "Kishor25");

            services.AddDbContext<ProductDbContext>(option =>
                option.UseSqlServer(connectionString));

            services.AddScoped<IProductService, ProductsRepository>();

            return services;
        }
    }
}
