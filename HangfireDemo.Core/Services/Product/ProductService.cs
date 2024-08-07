using Microsoft.EntityFrameworkCore;

namespace HangfireDemo.Core.Services.Product;

using PractiseForMason.Core.Domain;

public interface IProductService : IScopeService
{
    Task AddProductAsync(Domain.Product product, CancellationToken cancellationToken);
    
    Task<Domain.Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
}

public class ProductService : IProductService
{
    private readonly IProductDataProvider _dataProvider;

    public ProductService(IProductDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public async Task AddProductAsync(Domain.Product product, CancellationToken cancellationToken)
    {
        await _dataProvider.CreateAsync(product, cancellationToken).ConfigureAwait(false);
        
        await _dataProvider.SaveChangesAsync(cancellationToken);
    }

    public async Task<Domain.Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dataProvider.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
    }
}