using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oder.Infrastructure.OrderDBContext;
using Oder.Infrastructure.Repositries;
using Order.Core.RepositriesContract;


namespace Oder.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOrderInfrastucture(this IServiceCollection services,IConfiguration configuration) 
        {
            services.AddDbContext<OrderDbContext>(option =>
                option.UseSqlServer(configuration.GetConnectionString("OrderConn")));

            services.AddScoped<IOrdersRepository,OrdersRepository>();
            return services;
        }
    }
}
