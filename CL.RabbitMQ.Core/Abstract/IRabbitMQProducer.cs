using CL.RabbitMQ.Shared;
using System.Collections.Generic;

namespace CL.RabbitMQ.Core.Abstract
{
    public interface IRabbitMQProducer
    {
        Result Enqueue<T>(T model, string queueName)
            where T : class, new();

        Result Enqueue<T>(IEnumerable<T> models, string queueName)
            where T : class, new();
    }
}
