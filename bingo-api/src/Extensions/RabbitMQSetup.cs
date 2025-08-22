using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;

namespace bingo_api.src.Extensions;

public static class RabbitMQSetup
{
    public static void AddRabbitMQService(this IServiceCollection services)
    {
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ctx , cfg) =>
            {
                cfg.Host(new Uri("amqp://localhost:5672"), host =>
                {
                    host.Username("admin");
                    host.Password("admin123");
                });

                cfg.ConfigureEndpoints(ctx);
            });
        });

        
    }
}
