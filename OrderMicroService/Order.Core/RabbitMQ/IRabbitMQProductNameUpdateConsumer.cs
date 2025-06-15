using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.RabbitMQ
{
    public interface IRabbitMQProductNameUpdateConsumer
    {
        void Consume();
        void Dispose();
    }
}
