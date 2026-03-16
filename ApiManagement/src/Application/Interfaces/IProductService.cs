namespace Application.Interfaces;

using Application.DTOs;

/// <summary>
/// Service interface for Product operations
/// </summary>
public interface IProductService
{
    Task<ProductResponseDto?> GetProductByIdAsync(int id);
    Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
    Task<IEnumerable<ProductResponseDto>> GetProductsByCategoryAsync(int categoryId);
    Task<IEnumerable<ProductResponseDto>> GetActiveProductsAsync();
    Task<IEnumerable<ProductResponseDto>> GetOutOfStockProductsAsync();
    Task<ProductResponseDto?> GetProductBySkuAsync(string sku);
    Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto);
    Task<ProductResponseDto> UpdateProductAsync(UpdateProductDto dto);
    Task<ProductResponseDto> UpdateProductStockAsync(int productId, int quantity, string reason);
    Task<bool> DeleteProductAsync(int id);
}

/// <summary>
/// Service interface for Category operations
/// </summary>
public interface ICategoryService
{
    Task<CategoryResponseDto?> GetCategoryByIdAsync(int id);
    Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
    Task<IEnumerable<CategoryResponseDto>> GetActiveCategoriesAsync();
    Task<CategoryResponseDto?> GetCategoryByNameAsync(string name);
    Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto dto);
    Task<CategoryResponseDto> UpdateCategoryAsync(UpdateCategoryDto dto);
    Task<bool> DeleteCategoryAsync(int id);
}
