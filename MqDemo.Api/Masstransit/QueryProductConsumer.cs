using MassTransit;
using MqDemo.Api.Events.Product;
using ILogger = Serilog.ILogger;

namespace MqDemo.Api.Masstransit;

public class QueryProductConsumer : IConsumer<QueryProductEvent>
{
    private readonly ILogger _logger;

    public QueryProductConsumer(ILogger logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<QueryProductEvent> context)
    {
        // throw new ConsumerException("hello expection");en
        _logger.Information($"Product Name: {context.Message.Name}, Timestamp: {context.Message.Timestamp}");
        
        return Task.CompletedTask;
    }
}