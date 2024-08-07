using Hangfire;
using HangfireDemo.Core.Domain;
using HangfireDemo.Core.Services.Product;
using Microsoft.AspNetCore.Mvc;
using PractiseForMason.Core.Domain;

namespace HangfireDemo.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [Route("add"), HttpPost]
    public async Task<IResult> AddProduct([FromBody] Product product, CancellationToken cancellationToken = default)
    {
        await _productService.AddProductAsync(product, cancellationToken);
        // BackgroundJob.Enqueue<IProductService>(p => p.AddProductAsync(product, cancellationToken));
        // product.Id = Guid.NewGuid();
        // BackgroundJob.Enqueue<IProductService>(p => p.AddProductAsync(product, cancellationToken));
        RecurringJob.AddOrUpdate("easyjob", () => Console.Write("Easy!"), Cron.Daily);
        return Results.Ok();
    }

    [Route("get"), HttpGet]
    public async Task<Product?> GetProduct([FromQuery] Guid id, CancellationToken cancellationToken = default)
    {
        var productByIdAsync = await _productService.GetProductByIdAsync(id, cancellationToken);
        return productByIdAsync;
    }
}