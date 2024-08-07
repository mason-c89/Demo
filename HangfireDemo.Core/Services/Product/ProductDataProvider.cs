using Microsoft.EntityFrameworkCore;

namespace HangfireDemo.Core.Services.Product;

using PractiseForMason.Core.Domain;

public interface IProductDataProvider : IScopeService
{
    Task CreateAsync(Domain.Product product, CancellationToken cancellationToken);
    
    Task<Domain.Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}

public class ProductDataProvider : IProductDataProvider
{
    private readonly DbContext _dbContext;

    public ProductDataProvider(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(Domain.Product product, CancellationToken cancellationToken)
    {
        Console.WriteLine(_dbContext.Model.ToDebugString());
        await _dbContext.AddAsync(product, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Domain.Product> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Domain.Product>().Where(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}