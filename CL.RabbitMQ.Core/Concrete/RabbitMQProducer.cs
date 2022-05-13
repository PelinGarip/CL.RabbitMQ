using CL.RabbitMQ.Core.Abstract;
using CL.RabbitMQ.Core.Consts;
using CL.RabbitMQ.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CL.RabbitMQ.Core.Concrete
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly IRabbitMQService _rabbitMQService;
        public RabbitMQProducer(IRabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        public Result Enqueue<T>(T model, string queueName) where T : class, new()
        {
            var models = new List<T>();
            models.Add(model);

            return this.Enqueue<T>(models, queueName);
        }

        public Result Enqueue<T>(IEnumerable<T> models, string queueName) where T : class, new()
        {
            try
            {
                using (var channel = _rabbitMQService.GetConnection().CreateModel())
                {
                    channel.QueueDeclare
                    (
                        queue: queueName,
                        durable: true, // Bağlantı gittiğinde queue kalsın mı, false ise içindekiler ile birlikte uçar
                        exclusive: false, // Queue tanımlanırken geldiği connectiondan başka connection tanısın mı
                        autoDelete: false, // Tüm consumerlar dinlemeyi bıraktığında queue da silinsin mi
                        arguments: null
                    );

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.Expiration = RabbitMQConsts.MessagesTTL.ToString();

                    foreach (var model in models)
                    {
                        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
                        channel.BasicPublish
                        (
                            exchange: "",
                            routingKey: queueName,
                            mandatory: true,
                            basicProperties: properties,
                            body: body
                        );

                    }
                }

                return new Result(true, "OK");
            }
            catch (Exception ex)
            {
                // Loglayaydık iyiydi
                return new Result(false, ex.Message);
            }
        }
    }
}
