using System.ComponentModel;

namespace CL.RabbitMQ.Core.Enums
{
    public enum RabbitMQQueueNames
    {
        [Description("QueueNameEmail")]
        Email = 1,
        [Description("QueueNameSms")]
        SMS = 2
    }
}
