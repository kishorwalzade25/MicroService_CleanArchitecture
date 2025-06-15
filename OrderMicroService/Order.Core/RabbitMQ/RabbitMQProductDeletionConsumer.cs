using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Order.Core.RabbitMQ
{
    public class RabbitMQProductDeletionConsumer : IDisposable, IRabbitMQProductDeletionConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMQProductDeletionConsumer> _logger;
        private readonly IDistributedCache _cache;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQProductDeletionConsumer(IConfiguration configuration,ILogger<RabbitMQProductDeletionConsumer> logger,IDistributedCache cache) 
        {
            _configuration=configuration;
            _logger=logger;
            _cache = cache;

            string hostName = _configuration["RabbitMQ_HostName"]!;
            string userName = _configuration["RabbitMQ_UserName"]!;
            string password = _configuration["RabbitMQ_Password"]!;
            string port = _configuration["RabbitMQ_Port"]!;

            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                Port=Convert.ToInt32(port)
            };

            _connection=connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            
        }


        public void Consume() 
        {
            //for direct exchange
            //string routingKey = "product.delete";


            //for topic exchange
            //meaning listening to any "product.anyUpdateRoutingMatchingKey"
            //string routingKey = "product.#";

            //for header exchange
            //for cache updating
            var headers = new Dictionary<string, object>()
            {
              { "x-match", "all" },
              { "event", "product.delete" },
              { "RowCount", 1 }
            };


            string queueName = "orders.product.delete.queue";

            string exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;


            //for Direct Exchange
            //_channel.ExchangeDeclare(exchange:exchangeName,type:ExchangeType.Direct,durable:true);

            // for  "exchangeName"  the "ExchangeType" in all rabbitmq service mustbe same as publishing "ExchangeType"
            //for fanout
            //_channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout, durable: true);

            //for topic excahnge
            //for fanout
            //_channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic, durable: true);

            //for header exchange
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Headers, durable: true);

            _channel.QueueDeclare(queue:queueName,durable:true,exclusive:false,autoDelete:false,arguments:null);

            //for direct exchange
            //_channel.QueueBind(queue:queueName,exchange:exchangeName,routingKey:routingKey);

            //for fanout
            //_channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: string.Empty);

            //for topic exchange
            //_channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey:routingKey);

            //for header exchange
            _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: string.Empty,arguments:headers);

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

            consumer.Received +=  async(sender, args) =>
            {
                byte[] body = args.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);

                if (message != null)
                {
                    ProductDeletionMessage? productDeletionMessage = JsonSerializer.Deserialize<ProductDeletionMessage>(message);

                    if (productDeletionMessage != null)
                    {
                        _logger.LogInformation($"Product deleted: {productDeletionMessage.ProductID}, Product name: {productDeletionMessage.ProductName}");
                        await HandleProductDeletion(productDeletionMessage.ProductID);
                    }

                   
                }
            };

            _channel.BasicConsume(queue: queueName, consumer: consumer, autoAck: true);

        }

        private async Task HandleProductDeletion(int productID)
        {
            string cacheKeyToWrite = $"product:{productID}";

            await _cache.RemoveAsync(cacheKeyToWrite);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
