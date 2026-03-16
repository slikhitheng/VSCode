namespace Domain.Entities;

/// <summary>
/// Product Category entity
/// </summary>
public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public ICollection<Product> Products { get; set; } = new List<Product>();

    public Category() { }

    public Category(string name, string description = "")
    {
        Name = name;
        Description = description;
        IsActive = true;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
