using MassTransit;
using MqDemo.Api.Events.Product;

namespace MqDemo.Api.Masstransit;

public class QueryProductConsumer : IConsumer<QueryProductEvent>
{
    public Task Consume(ConsumeContext<QueryProductEvent> context)
    {
        Console.WriteLine($"Product Name: {context.Message.Name}, Timestamp: {context.Message.Timestamp}");
        
        return Task.CompletedTask;
    }
}