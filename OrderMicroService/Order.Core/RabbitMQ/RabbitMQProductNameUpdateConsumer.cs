using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Order.Core.DTO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Order.Core.RabbitMQ
{
    public class RabbitMQProductNameUpdateConsumer : IRabbitMQProductNameUpdateConsumer, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMQProductNameUpdateConsumer> _logger;
        private readonly IDistributedCache _cache;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQProductNameUpdateConsumer(IConfiguration configuration, ILogger<RabbitMQProductNameUpdateConsumer> logger,IDistributedCache cache) 
        {
            _configuration = configuration;
            _logger = logger;
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
                Port = Convert.ToInt32(port)
            };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Consume()
        {
            // for direct exchange
            //string routingKey = "product.update.name";


            //for topic exchange
            //meaning listening to any "product.update.anyUpdateRoutingMatchingKey"
            //string routingKey = "product.update.*";

            //for header exchange
            //var headers = new Dictionary<string, object>()
            //{
            //     { "x-match", "all" },
            //     { "event", "product.update" },
            //     { "field", "name" },
            //     { "RowCount", 1 }
            //};


            //for cache updating
            var headers = new Dictionary<string, object>()
            {
                 { "x-match", "all" },
                 { "event", "product.update" },
                 
                 { "RowCount", 1 }
            };

            string queueName = "orders.product.update.name.queue";

            // create exchange

            string exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;

            //for direct Exchange
            //_channel.ExchangeDeclare(exchange:exchangeName,type:ExchangeType.Direct,durable:true);

            // for fanout exchange
            //_channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout, durable: true);

            //for topic exchange
            // for fanout exchange
            //_channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic, durable: true);

            //for header exchange
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Headers, durable: true);

            //create message queue
            _channel.QueueDeclare(queue:queueName,durable:true,exclusive:false,autoDelete:false,arguments:null);

            // argument
            //x-message-ttl | x-max-length | x-expired

            // Bind the message to exchange
            // for direct exchange
            //_channel.QueueBind(queue:queueName,exchange:exchangeName,routingKey:routingKey);

            //for fanout exchange
            //_channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey:string.Empty);

            //for topic exchange
            //_channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

            //for headers
            _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: string.Empty, arguments: headers);


            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

            //for header
            // consumer.Received +=  (sender, args) =>

            //for cache update
            consumer.Received += async (sender, args) =>
            {
                byte[] body = args.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);

                if (message != null)
                {
                    //ProductNameUpdateMessage? productNameUpdateMessage = JsonSerializer.Deserialize<ProductNameUpdateMessage>(message);
                    //_logger.LogInformation($"Product name updated: {productNameUpdateMessage?.ProductID}, New name: {productNameUpdateMessage?.NewName}");

                    //for cache updating
                    ProductDTO? productDTO = JsonSerializer.Deserialize<ProductDTO>(message);

                    if (productDTO != null) 
                    {
                        await HandleProductUpdation(productDTO);
                    }
                }
            };

            _channel.BasicConsume(queue: queueName, consumer: consumer, autoAck: true);

        }

        private async Task HandleProductUpdation(ProductDTO productDTO)
        {
            _logger.LogInformation($"Product name updated:{productDTO.ProductID},New name:{productDTO.ProductName}");
            string productJson=JsonSerializer.Serialize(productDTO);

            DistributedCacheEntryOptions options =  new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(300));
            string cacheKeyToWrite=$"product:{productDTO.ProductID}";
            await _cache.SetStringAsync(cacheKeyToWrite,productJson,options);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
