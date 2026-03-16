namespace Domain.Exceptions;

/// <summary>
/// Base exception for domain-specific errors
/// </summary>
public class DomainException : Exception
{
    public string? Code { get; set; }

    public DomainException(string message, string? code = null) : base(message)
    {
        Code = code;
    }

    public DomainException(string message, Exception innerException, string? code = null) 
        : base(message, innerException)
    {
        Code = code;
    }
}

/// <summary>
/// Exception thrown when an entity is not found
/// </summary>
public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityName, int id) 
        : base($"{entityName} with id '{id}' not found", "ENTITY_NOT_FOUND") { }

    public EntityNotFoundException(string entityName, string value) 
        : base($"{entityName} with value '{value}' not found", "ENTITY_NOT_FOUND") { }
}

/// <summary>
/// Exception thrown when validation fails
/// </summary>
public class ValidationException : DomainException
{
    public ValidationException(string message, string? code = null) 
        : base(message, code ?? "VALIDATION_ERROR") { }
}

/// <summary>
/// Exception thrown for business rule violations
/// </summary>
public class BusinessRuleException : DomainException
{
    public BusinessRuleException(string message, string? code = null) 
        : base(message, code ?? "BUSINESS_RULE_VIOLATION") { }
}

/// <summary>
/// Exception for insufficient stock
/// </summary>
public class InsufficientStockException : BusinessRuleException
{
    public InsufficientStockException(string productName, int requested, int available)
        : base($"Insufficient stock for {productName}. Requested: {requested}, Available: {available}", "INSUFFICIENT_STOCK") { }
}

/// <summary>
/// Exception for duplicate entity
/// </summary>
public class DuplicateEntityException : BusinessRuleException
{
    public DuplicateEntityException(string entityName, string value)
        : base($"{entityName} with '{value}' already exists", "DUPLICATE_ENTITY") { }
}
