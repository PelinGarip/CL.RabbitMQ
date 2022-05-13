using CL.RabbitMQ.Core.Enums;
using CL.RabbitMQ.Shared;
using CL.RabbitMQ.Shared.Model;

namespace CL.RabbitMQ.Services.Abstract
{
    public interface INotificationService
    {
        Result Send(NotificationTypes notificationType, NotificationModel notificationModel);
    }
}
