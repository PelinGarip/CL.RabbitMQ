using RabbitMQ.Client;

namespace CL.RabbitMQ.Core.Abstract
{
    public interface IRabbitMQService
    {
        IConnection GetConnection();
        IModel GetModel(IConnection connection);
    }
}
