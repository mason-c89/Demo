namespace MqDemo.Api.Events.Product;

public class QueryProductEvent
{
    public Guid Id { get; init; }
    public DateTime Timestamp { get; init; }
    public string Name { get; init; }
}