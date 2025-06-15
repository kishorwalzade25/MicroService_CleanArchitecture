using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.RabbitMQ
{
    public interface IRabbitMQPublisher
    {
        //for direct exchange
        //void Publish<T>(string routingKey,T message);

        //for header exchange 
        void Publish<T>(Dictionary<string, object> headers, T message);
    }
}
