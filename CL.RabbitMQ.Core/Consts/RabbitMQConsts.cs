namespace CL.RabbitMQ.Core.Consts
{
    public class RabbitMQConsts
    {
        /// yaşam süresi
        public const int MessagesTTL = 1000 * 60 * 60 * 2;

        //Aynı anda - Eşzamanlı e-posta gönderimi sayısı, thread açma için sınırı belirleriz
        public const int ParallelThreadsCount = 3;
    }
}
