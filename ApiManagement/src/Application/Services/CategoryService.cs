namespace Application.Services;

using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Application.DTOs;
using Application.Interfaces;

/// <summary>
/// Service for managing Categories
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<CategoryResponseDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        return category == null ? null : MapToDto(category);
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        return categories.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetActiveCategoriesAsync()
    {
        var categories = await _unitOfWork.Categories.GetActiveCategoriesAsync();
        return categories.Select(MapToDto).ToList();
    }

    public async Task<CategoryResponseDto?> GetCategoryByNameAsync(string name)
    {
        var category = await _unitOfWork.Categories.GetByNameAsync(name);
        return category == null ? null : MapToDto(category);
    }

    public async Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ValidationException("Category name is required");

        // Check for duplicate
        var existing = await _unitOfWork.Categories.GetByNameAsync(dto.Name);
        if (existing != null)
            throw new DuplicateEntityException("Category", dto.Name);

        var category = new Category(dto.Name, dto.Description);

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Category created: {CategoryName}", dto.Name);
        return MapToDto(category);
    }

    public async Task<CategoryResponseDto> UpdateCategoryAsync(UpdateCategoryDto dto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(dto.Id)
            ?? throw new EntityNotFoundException(nameof(Category), dto.Id);

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ValidationException("Category name is required");

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Category updated: {CategoryId} - {CategoryName}", dto.Id, dto.Name);
        return MapToDto(category);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var exists = await _unitOfWork.Categories.ExistsAsync(id);
        if (!exists)
            throw new EntityNotFoundException(nameof(Category), id);

        return await _unitOfWork.Categories.DeleteAsync(id);
    }

    private static CategoryResponseDto MapToDto(Category entity)
    {
        return new CategoryResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive,
            ProductCount = entity.Products?.Count ?? 0,
            CreatedAt = entity.CreatedAt
        };
    }
}
