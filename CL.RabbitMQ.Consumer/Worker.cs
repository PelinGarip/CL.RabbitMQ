using CL.RabbitMQ.Core;
using CL.RabbitMQ.Core.Consts;
using CL.RabbitMQ.Core.Enums;
using CL.RabbitMQ.Core.Model;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CL.RabbitMQ.Consumer
{
    public class Worker : BackgroundService
    {
        private IConnection connection;
        private IModel channel;
        private readonly RabbitMQConfiguration _rabbitMQConfiguration;
        public Worker(RabbitMQConfiguration rabbitMQConfiguration)
        {
            _rabbitMQConfiguration = rabbitMQConfiguration;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _rabbitMQConfiguration.HostName,
                    Port = _rabbitMQConfiguration.Port,
                    VirtualHost = _rabbitMQConfiguration.VirtualHost,
                    UserName = _rabbitMQConfiguration.UserName,
                    Password = _rabbitMQConfiguration.Password
                };
                factory.AutomaticRecoveryEnabled = true;
                factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
                factory.TopologyRecoveryEnabled = false;

                connection = factory.CreateConnection();

                channel = connection.CreateModel();
                channel.QueueDeclare
                (
                    queue: RabbitMQQueueNames.Email.ToString(),
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                channel.BasicQos(0, RabbitMQConsts.ParallelThreadsCount, false);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += EmailConsumer_Received;
                await Task.FromResult
                (
                    channel.BasicConsume
                    (
                        queue: RabbitMQQueueNames.Email.ToString(),
                        autoAck: false, // true ise mesaj kuyruğa düştü mü diye bakmış oluruz, false ise kuyruğa düştü mü ve Consumer_Received metodu düzgün bi şekilde çalıştı mı diye bakmış oluruz
                        consumerTag: "",
                        noLocal: false,
                        exclusive: false,
                        arguments: null,
                        consumer: consumer
                    )
                );
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
            }
        }

        private void EmailConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = Encoding.UTF8.GetString(e.Body.ToArray());
                var message = JsonConvert.DeserializeObject<RabbitMQMessageModel>(body);

                Console.WriteLine($"Message Received: {body}");

                // Mail at

                channel.BasicAck(e.DeliveryTag, false); // Burada RabbitMQ'ya mesajı aldığımızı ve başarılı bir şekilde işlediğimizi, artık silebileceğini söylüyoruz.

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            channel.Close();
            connection.Close();

            return Task.CompletedTask;
        }
    }
}
