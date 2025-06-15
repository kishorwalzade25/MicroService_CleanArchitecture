using Microsoft.Extensions.DependencyInjection;
using Product.Core.RabbitMQ;

namespace Product.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services) 
        {
            services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();
            
            return services;
        }
    }
}
