namespace Domain.Events;

/// <summary>
/// Base domain event class
/// </summary>
public abstract class DomainEvent
{
    public DateTime OccurredOn { get; }
    public Guid AggregateId { get; }

    protected DomainEvent(Guid aggregateId)
    {
        AggregateId = aggregateId;
        OccurredOn = DateTime.UtcNow;
    }
}

/// <summary>
/// Event fired when product is created
/// </summary>
public class ProductCreatedDomainEvent : DomainEvent
{
    public string ProductName { get; set; }
    public string Sku { get; set; }
    public decimal Price { get; set; }

    public ProductCreatedDomainEvent(Guid aggregateId, string productName, string sku, decimal price)
        : base(aggregateId)
    {
        ProductName = productName;
        Sku = sku;
        Price = price;
    }
}

/// <summary>
/// Event fired when product stock changes
/// </summary>
public class ProductStockChangedDomainEvent : DomainEvent
{
    public int PreviousQuantity { get; set; }
    public int NewQuantity { get; set; }
    public string ChangeReason { get; set; }

    public ProductStockChangedDomainEvent(Guid aggregateId, int previousQuantity, int newQuantity, string reason)
        : base(aggregateId)
    {
        PreviousQuantity = previousQuantity;
        NewQuantity = newQuantity;
        ChangeReason = reason;
    }
}

/// <summary>
/// Event fired when product price is updated
/// </summary>
public class ProductPriceChangedDomainEvent : DomainEvent
{
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }

    public ProductPriceChangedDomainEvent(Guid aggregateId, decimal oldPrice, decimal newPrice)
        : base(aggregateId)
    {
        OldPrice = oldPrice;
        NewPrice = newPrice;
    }
}
