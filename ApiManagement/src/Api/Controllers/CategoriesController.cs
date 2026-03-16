namespace Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;
using Api.Middleware;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<CategoryResponseDto>>>> GetAllCategories()
    {
        _logger.LogInformation("Retrieving all categories");
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(new SuccessResponse<IEnumerable<CategoryResponseDto>> { Data = categories, Message = "Categories retrieved successfully" });
    }

    [HttpGet("active")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<CategoryResponseDto>>>> GetActiveCategories()
    {
        _logger.LogInformation("Retrieving active categories");
        var categories = await _categoryService.GetActiveCategoriesAsync();
        return Ok(new SuccessResponse<IEnumerable<CategoryResponseDto>> { Data = categories, Message = "Active categories retrieved successfully" });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SuccessResponse<CategoryResponseDto>>> GetCategoryById(int id)
    {
        _logger.LogInformation("Retrieving category with ID: {Id}", id);
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound(new ErrorResponse { Message = $"Category with ID {id} not found", Code = "CATEGORY_NOT_FOUND" });
        return Ok(new SuccessResponse<CategoryResponseDto> { Data = category, Message = "Category retrieved successfully" });
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<CategoryResponseDto>>> CreateCategory([FromBody] CreateCategoryDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        _logger.LogInformation("Creating new category: {Name}", dto.Name);
        var createdCategory = await _categoryService.CreateCategoryAsync(dto);
        return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id },
            new SuccessResponse<CategoryResponseDto> { Data = createdCategory, Message = "Category created successfully" });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SuccessResponse<CategoryResponseDto>>> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        dto.Id = id;
        _logger.LogInformation("Updating category with ID: {Id}", id);
        var updatedCategory = await _categoryService.UpdateCategoryAsync(dto);
        return Ok(new SuccessResponse<CategoryResponseDto> { Data = updatedCategory, Message = "Category updated successfully" });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        _logger.LogInformation("Deleting category with ID: {Id}", id);
        var result = await _categoryService.DeleteCategoryAsync(id);
        if (!result) return NotFound(new ErrorResponse { Message = $"Category with ID {id} not found", Code = "CATEGORY_NOT_FOUND" });
        return NoContent();
    }
}
