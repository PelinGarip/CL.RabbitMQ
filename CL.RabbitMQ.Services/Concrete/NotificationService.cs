using CL.RabbitMQ.Core.Abstract;
using CL.RabbitMQ.Core.Enums;
using CL.RabbitMQ.Core.Model;
using CL.RabbitMQ.Services.Abstract;
using CL.RabbitMQ.Shared;
using CL.RabbitMQ.Shared.Model;
using System;

namespace CL.RabbitMQ.Services.Concrete
{
    public class NotificationService: INotificationService
    {
        private readonly IRabbitMQProducer _rabbitMQProducer;
        public NotificationService(IRabbitMQProducer rabbitMQProducer)
        {
            _rabbitMQProducer = rabbitMQProducer;
        }

        public Result Send(NotificationTypes notificationType, NotificationModel notificationModel)
        {
            try
            {
                // Ah bi factory olaydı
                var queueName = notificationType == NotificationTypes.Email ?
                    RabbitMQQueueNames.Email :
                    RabbitMQQueueNames.SMS;

                var messageModel = new RabbitMQMessageModel
                {
                    NotificationType = notificationType,
                    NotificationModel = notificationModel
                };

                return _rabbitMQProducer.Enqueue<RabbitMQMessageModel>(messageModel, queueName.ToString());

            }
            catch (Exception ex)
            {
                // Loglayamadım ya, dert oldu
                return new Result(false, ex.Message);
            }
        }
    }
}
