using CL.RabbitMQ.Core.Abstract;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;

namespace CL.RabbitMQ.Core.Concrete
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly RabbitMQConfiguration _rabbitMQConfiguration;
        public RabbitMQService(RabbitMQConfiguration rabbitMQConfiguration)
        {
            _rabbitMQConfiguration = rabbitMQConfiguration;
        }

        public IConnection GetConnection()
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

                // Otomatik bağlantı
                factory.AutomaticRecoveryEnabled = true;

                // 10 snde bir tekrar bağlantı
                factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);

                // Bağlantı kesildiğinde mesaj tüketimini durdur
                factory.TopologyRecoveryEnabled = false;

                return factory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                // Bağlantı gitti, napalım? Tekrar deneyelim.

                return GetConnection();

                // Allahını seven log atsın
            }
        }

        public IModel GetModel(IConnection connection)
        {
            return connection.CreateModel();
        }
    }
}
