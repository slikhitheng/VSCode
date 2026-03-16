namespace Domain.Interfaces;

using Domain.Entities;

/// <summary>
/// Unit of Work pattern interface for managing repositories
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
