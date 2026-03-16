namespace Application.Services;

using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Application.DTOs;
using Application.Interfaces;

/// <summary>
/// Service for managing Products
/// </summary>
public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        return product == null ? null : MapToDto(product);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        return products.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<ProductResponseDto>> GetProductsByCategoryAsync(int categoryId)
    {
        var products = await _unitOfWork.Products.GetByCategoryAsync(categoryId);
        return products.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<ProductResponseDto>> GetActiveProductsAsync()
    {
        var products = await _unitOfWork.Products.GetActiveProductsAsync();
        return products.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<ProductResponseDto>> GetOutOfStockProductsAsync()
    {
        var products = await _unitOfWork.Products.GetOutOfStockProductsAsync();
        return products.Select(MapToDto).ToList();
    }

    public async Task<ProductResponseDto?> GetProductBySkuAsync(string sku)
    {
        var product = await _unitOfWork.Products.GetBySkuAsync(sku);
        return product == null ? null : MapToDto(product);
    }

    public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto)
    {
        ValidateProductDto(dto);

        // Check if SKU already exists
        var existingSku = await _unitOfWork.Products.GetBySkuAsync(dto.Sku);
        if (existingSku != null)
            throw new DuplicateEntityException("Product SKU", dto.Sku);

        // Verify category exists
        var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
        if (category == null)
            throw new EntityNotFoundException(nameof(Category), dto.CategoryId);

        var product = new Product(dto.Name, dto.Sku, dto.Price, dto.CategoryId, dto.QuantityInStock)
        {
            Description = dto.Description,
            ImageUrl = dto.ImageUrl
        };

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Product created: {ProductName} ({Sku})", dto.Name, dto.Sku);
        return MapToDto(product);
    }

    public async Task<ProductResponseDto> UpdateProductAsync(UpdateProductDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(dto.Id)
            ?? throw new EntityNotFoundException(nameof(Product), dto.Id);

        ValidateUpdateDto(dto);

        // Verify category exists if changed
        if (product.CategoryId != dto.CategoryId)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
            if (category == null)
                throw new EntityNotFoundException(nameof(Category), dto.CategoryId);
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.CategoryId = dto.CategoryId;
        if (dto.Price != product.Price)
            product.UpdatePrice(dto.Price);
        product.ImageUrl = dto.ImageUrl;
        product.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Product updated: {ProductId} - {ProductName}", dto.Id, dto.Name);
        return MapToDto(product);
    }

    public async Task<ProductResponseDto> UpdateProductStockAsync(int productId, int quantity, string reason)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId)
            ?? throw new EntityNotFoundException(nameof(Product), productId);

        if (quantity == 0)
            throw new ValidationException("Quantity cannot be zero");

        try
        {
            if (quantity > 0)
                product.IncreaseStock(quantity);
            else
                product.ReduceStock(Math.Abs(quantity));

            product.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Product stock updated: {ProductId}, Quantity change: {Quantity}, Reason: {Reason}", 
                productId, quantity, reason);

            return MapToDto(product);
        }
        catch (InvalidOperationException ex)
        {
            throw new BusinessRuleException(ex.Message);
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var exists = await _unitOfWork.Products.ExistsAsync(id);
        if (!exists)
            throw new EntityNotFoundException(nameof(Product), id);

        return await _unitOfWork.Products.DeleteAsync(id);
    }

    private static void ValidateProductDto(CreateProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ValidationException("Product name is required");

        if (string.IsNullOrWhiteSpace(dto.Sku))
            throw new ValidationException("Product SKU is required");

        if (dto.Price < 0)
            throw new ValidationException("Product price cannot be negative");

        if (dto.CategoryId <= 0)
            throw new ValidationException("Valid category ID is required");

        if (dto.QuantityInStock < 0)
            throw new ValidationException("Quantity cannot be negative");
    }

    private static void ValidateUpdateDto(UpdateProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ValidationException("Product name is required");

        if (dto.Price < 0)
            throw new ValidationException("Product price cannot be negative");

        if (dto.CategoryId <= 0)
            throw new ValidationException("Valid category ID is required");
    }

    private static ProductResponseDto MapToDto(Product entity)
    {
        return new ProductResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Sku = entity.Sku,
            Price = entity.Price,
            CategoryId = entity.CategoryId,
            CategoryName = entity.Category?.Name,
            QuantityInStock = entity.QuantityInStock,
            IsActive = entity.IsActive,
            IsOutOfStock = entity.IsOutOfStock(),
            ImageUrl = entity.ImageUrl,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
