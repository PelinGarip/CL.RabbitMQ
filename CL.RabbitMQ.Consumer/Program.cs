using CL.RabbitMQ.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CL.RabbitMQ.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    services.AddSingleton(f =>
                    {
                        return configuration.GetSection("RabbitMQConfiguration").Get<RabbitMQConfiguration>();
                    });

                    services.AddHostedService<Worker>();
                });
    }
}
