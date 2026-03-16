namespace Domain.Entities;

/// <summary>
/// Product aggregate entity - core domain entity
/// </summary>
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public int QuantityInStock { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }

    public Product() { }

    public Product(string name, string sku, decimal price, int categoryId, int quantityInStock)
    {
        Name = name;
        Sku = sku;
        Price = price;
        CategoryId = categoryId;
        QuantityInStock = quantityInStock;
        IsActive = true;
    }

    // Domain methods with business rules
    public bool IsOutOfStock() => QuantityInStock <= 0;

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidOperationException("Quantity must be positive");
        
        if (QuantityInStock < quantity)
            throw new InvalidOperationException("Insufficient stock");
        
        QuantityInStock -= quantity;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidOperationException("Quantity must be positive");
        
        QuantityInStock += quantity;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new InvalidOperationException("Price cannot be negative");
        
        Price = newPrice;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
