using Autofac;
using HangfireDemo.Core.Data;
using HangfireDemo.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace HangfireDemo.Core;

public class HangfireDemoModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterDependency(builder);
        
        builder.RegisterType<ProductContext>()
            .AsSelf()
            .As<DbContext>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        base.Load(builder);
    }

    private void RegisterDependency(ContainerBuilder builder)
    {foreach (var type in typeof(IService).Assembly.GetTypes()
                  .Where(t => typeof(IService).IsAssignableFrom(t) && t.IsClass))
        {
            switch (type)
            {
                case var t when typeof(IScopeService).IsAssignableFrom(type):
                    builder.RegisterType(t).AsImplementedInterfaces().InstancePerLifetimeScope();
                    break;
                case var t when typeof(ISingletonService).IsAssignableFrom(type):
                    builder.RegisterType(t).AsImplementedInterfaces().SingleInstance();
                    break;
                case var t when typeof(ITransientService).IsAssignableFrom(type):
                    builder.RegisterType(t).AsImplementedInterfaces().InstancePerDependency();
                    break;
                default:
                    builder.RegisterType(type).AsImplementedInterfaces();
                    break;
            }
        }
        
    }
}