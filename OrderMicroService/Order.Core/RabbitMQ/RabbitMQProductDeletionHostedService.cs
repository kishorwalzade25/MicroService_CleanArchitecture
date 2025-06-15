using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.RabbitMQ
{
    public class RabbitMQProductDeletionHostedService : IHostedService
    {
        private readonly IRabbitMQProductDeletionConsumer _productNameUpdateConsumer;

        public RabbitMQProductDeletionHostedService(IRabbitMQProductDeletionConsumer rabbitMQProductDeletionConsumer) 
        {
            _productNameUpdateConsumer=rabbitMQProductDeletionConsumer; 
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _productNameUpdateConsumer.Consume();
            return Task.CompletedTask;  
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _productNameUpdateConsumer.Dispose();
            return Task.CompletedTask;
        }
    }
}
