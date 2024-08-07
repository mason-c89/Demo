using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;

namespace MqDemo.Api.Extension;

public static class MassTransitDependency
{
    public static void RegisterMultiBus(this ContainerBuilder builder, IConfiguration configuration,
        params Assembly[] assemblies)
    {
        if (assemblies == null || !assemblies.Any())
        {
            throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan.");
        }

        var services = new ServiceCollection();
        
        var scanTypes = assemblies.SelectMany(a => a.GetTypes());

        services.AddMassTransit(x =>
        {
            var interfaceType = typeof(IConsumer);
            var consumers = scanTypes
                .Where(type => type.IsClass && interfaceType.IsAssignableFrom(type) && !type.IsAbstract)
                .ToList();

            foreach (var consumer in consumers)
            {
                x.AddConsumer(consumer);
            }

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h => { });

                // 配置FileReceived的接收端点
                cfg.ReceiveEndpoint("demo_queue", e =>
                {
                    foreach (var consumer in consumers)
                    {
                        e.ConfigureConsumer(context, consumer);
                    }
                });
            });
        });

        if (services.Any())
        {
            builder.Populate(services);
        }
    }
}