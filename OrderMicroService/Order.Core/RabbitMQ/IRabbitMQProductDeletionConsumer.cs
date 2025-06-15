namespace Order.Core.RabbitMQ
{
    public interface IRabbitMQProductDeletionConsumer
    {
        void Consume();
        void Dispose();
    }
}