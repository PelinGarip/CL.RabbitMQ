using CL.RabbitMQ.Core.Enums;
using CL.RabbitMQ.Shared.Model;

namespace CL.RabbitMQ.Core.Model
{
    public class RabbitMQMessageModel
    {
        public NotificationTypes NotificationType { get; set; }
        public NotificationModel NotificationModel { get; set; }
    }
}
