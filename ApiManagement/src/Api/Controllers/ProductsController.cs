namespace Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;
using Api.Middleware;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<ProductResponseDto>>>> GetAllProducts()
    {
        _logger.LogInformation("Retrieving all products");
        var products = await _productService.GetAllProductsAsync();
        return Ok(new SuccessResponse<IEnumerable<ProductResponseDto>> { Data = products, Message = "Products retrieved successfully" });
    }

    [HttpGet("active")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<ProductResponseDto>>>> GetActiveProducts()
    {
        _logger.LogInformation("Retrieving active products");
        var products = await _productService.GetActiveProductsAsync();
        return Ok(new SuccessResponse<IEnumerable<ProductResponseDto>> { Data = products, Message = "Active products retrieved successfully" });
    }

    [HttpGet("out-of-stock")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<ProductResponseDto>>>> GetOutOfStockProducts()
    {
        _logger.LogInformation("Retrieving out of stock products");
        var products = await _productService.GetOutOfStockProductsAsync();
        return Ok(new SuccessResponse<IEnumerable<ProductResponseDto>> { Data = products, Message = "Out of stock products retrieved successfully" });
    }

    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<ProductResponseDto>>>> GetProductsByCategory(int categoryId)
    {
        _logger.LogInformation("Retrieving products for category: {CategoryId}", categoryId);
        var products = await _productService.GetProductsByCategoryAsync(categoryId);
        return Ok(new SuccessResponse<IEnumerable<ProductResponseDto>> { Data = products, Message = "Products retrieved successfully" });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SuccessResponse<ProductResponseDto>>> GetProductById(int id)
    {
        _logger.LogInformation("Retrieving product with ID: {Id}", id);
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound(new ErrorResponse { Message = $"Product with ID {id} not found", Code = "PRODUCT_NOT_FOUND" });
        return Ok(new SuccessResponse<ProductResponseDto> { Data = product, Message = "Product retrieved successfully" });
    }

    [HttpGet("sku/{sku}")]
    public async Task<ActionResult<SuccessResponse<ProductResponseDto>>> GetProductBySku(string sku)
    {
        _logger.LogInformation("Retrieving product with SKU: {Sku}", sku);
        var product = await _productService.GetProductBySkuAsync(sku);
        if (product == null)
            return NotFound(new ErrorResponse { Message = $"Product with SKU '{sku}' not found", Code = "PRODUCT_NOT_FOUND" });
        return Ok(new SuccessResponse<ProductResponseDto> { Data = product, Message = "Product retrieved successfully" });
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<ProductResponseDto>>> CreateProduct([FromBody] CreateProductDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        _logger.LogInformation("Creating new product: {Name}", dto.Name);
        var createdProduct = await _productService.CreateProductAsync(dto);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, 
            new SuccessResponse<ProductResponseDto> { Data = createdProduct, Message = "Product created successfully" });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SuccessResponse<ProductResponseDto>>> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        dto.Id = id;
        _logger.LogInformation("Updating product with ID: {Id}", id);
        var updatedProduct = await _productService.UpdateProductAsync(dto);
        return Ok(new SuccessResponse<ProductResponseDto> { Data = updatedProduct, Message = "Product updated successfully" });
    }

    [HttpPost("{id:int}/adjust-stock")]
    public async Task<ActionResult<SuccessResponse<ProductResponseDto>>> AdjustStock(int id, [FromBody] UpdateProductStockDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        _logger.LogInformation("Adjusting stock for product {Id}: {Quantity} ({Reason})", id, dto.Quantity, dto.Reason);
        var updatedProduct = await _productService.UpdateProductStockAsync(id, dto.Quantity, dto.Reason);
        return Ok(new SuccessResponse<ProductResponseDto> { Data = updatedProduct, Message = "Product stock updated successfully" });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        _logger.LogInformation("Deleting product with ID: {Id}", id);
        var result = await _productService.DeleteProductAsync(id);
        if (!result) return NotFound(new ErrorResponse { Message = $"Product with ID {id} not found", Code = "PRODUCT_NOT_FOUND" });
        return NoContent();
    }
}
