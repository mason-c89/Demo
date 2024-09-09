using Autofac;
using ILogger = Serilog.ILogger;

namespace MqDemo.Api;

public class LogModule : Module
{
    private readonly ILogger _logger;

    public LogModule(ILogger logger)
    {
        _logger = logger;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(_logger).AsSelf().AsImplementedInterfaces().SingleInstance();
        base.Load(builder);
    }
}