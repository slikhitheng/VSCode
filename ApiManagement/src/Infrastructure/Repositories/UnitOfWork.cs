namespace Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore.Storage;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

/// <summary>
/// Unit of Work pattern implementation for managing repositories and transactions
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ProductManagementDbContext _context;
    private IDbContextTransaction? _transaction;
    private IProductRepository? _productRepository;
    private ICategoryRepository? _categoryRepository;

    public UnitOfWork(ProductManagementDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IProductRepository Products
    {
        get { return _productRepository ??= new ProductRepository(_context); }
    }

    public ICategoryRepository Categories
    {
        get { return _categoryRepository ??= new CategoryRepository(_context); }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }
}
