using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MqDemo.Api.Events.Product;

namespace MqDemo.Api.controller;

public class ProductController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ProductController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [Route("get"), HttpGet]
    public async Task<IActionResult> QueryProductAsync([FromQuery] QueryProductEvent request)
    {
        await _publishEndpoint.Publish<QueryProductEvent>(new QueryProductEvent
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Timestamp = DateTime.Now
        });

        return Ok();
    }
}