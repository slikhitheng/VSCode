# Product Management API - Domain-Driven Design

A comprehensive, production-ready Web API for Product Management built with Domain-Driven Design (DDD), Entity Framework Core, and global exception handling.

## 🏗️ Project Architecture

### 4-Layer Architecture

```
┌────────────────────────────────────────────┐
│   API Layer (Presentation)                 │
│  - Controllers (Products, Categories)      │
│  - Middleware (Exception Handling)         │
│  - Dependency Injection Setup              │
└────────────────────────────────────────────┘
                   ↓
┌────────────────────────────────────────────┐
│   Application Layer (Business Logic)       │
│  - Services (ProductService, CategoryService)
│  - DTOs (Data Transfer Objects)            │
│  - Use Cases                               │
└────────────────────────────────────────────┘
                   ↓
┌────────────────────────────────────────────┐
│   Domain Layer (Core Business Rules)       │
│  - Entities (Product, Category)            │
│  - Interfaces                              │
│  - Domain Exceptions                       │
│  - Domain Events                           │
└────────────────────────────────────────────┘
                   ↓
┌────────────────────────────────────────────┐
│   Infrastructure Layer (Data Access)       │
│  - Entity Framework Core Context           │
│  - Repositories                            │
│  - Unit of Work Pattern                    │
└────────────────────────────────────────────┘
```

## 📂 Project Structure

```
ApiManagement/
├── src/
│   ├── Domain/                          # Core business logic
│   │   ├── Entities/
│   │   │   ├── BaseEntity.cs
│   │   │   ├── Product.cs               # Main aggregate
│   │   │   └── Category.cs
│   │   ├── Interfaces/
│   │   │   ├── IRepository.cs
│   │   │   └── IUnitOfWork.cs
│   │   ├── Exceptions/                  # Custom exceptions
│   │   │   └── DomainException.cs
│   │   ├── Events/                      # Domain events
│   │   │   └── DomainEvent.cs
│   │   └── Domain.csproj
│   │
│   ├── Application/                     # Business rules
│   │   ├── DTOs/
│   │   │   ├── ProductDto.cs
│   │   │   └── CategoryDto.cs
│   │   ├── Services/
│   │   │   ├── ProductService.cs
│   │   │   └── CategoryService.cs
│   │   ├── Interfaces/
│   │   │   └── IProductService.cs
│   │   └── Application.csproj
│   │
│   ├── Infrastructure/                  # Data access
│   │   ├── Data/
│   │   │   └── ProductManagementDbContext.cs
│   │   ├── Repositories/
│   │   │   ├── RepositoryImplementations.cs
│   │   │   └── UnitOfWork.cs
│   │   └── Infrastructure.csproj
│   │
│   └── Api/                             # REST API
│       ├── Controllers/
│       │   ├── ProductsController.cs     # 7 endpoints
│       │   └── CategoriesController.cs   # 6 endpoints
│       ├── Middleware/
│       │   └── ExceptionHandlingMiddleware.cs
│       ├── Extensions/
│       │   └── ServicesExtensions.cs
│       ├── Program.cs
│       ├── appsettings.json
│       ├── appsettings.Development.json
│       └── Api.csproj
│
├── ApiManagement.sln
└── README.md
```

## ✨ Key Features

### ✅ Domain-Driven Design (DDD)
- Clear separation of concerns across 4 layers
- Rich domain models with business logic encapsulation
- Entities with domain methods (not anemic models)
- Product aggregate with stock management
- Category support with relationships

### ✅ Entity Framework Core 8.0
- Code-first database approach
- Fluent configuration with ModelBuilder
- Automatic migrations support
- Soft delete pattern implementation
- Query filters for IsDeleted
- Navigation property autoinclude

### ✅ Global Exception Handling
- Centralized middleware
- 7 exception types with HTTP status codes:
  - `EntityNotFoundException` → 404
  - `ValidationException` → 400
  - `BusinessRuleException` → 422
  - `InsufficientStockException` → 422
  - `DuplicateEntityException` → 422
  - `DomainException` → 400
  - Unhandled → 500
- Standardized error responses with codes

### ✅ REST API Endpoints (13 Total)

**Products (7 endpoints):**
- `GET /api/products` - All products
- `GET /api/products/active` - Active only
- `GET /api/products/out-of-stock` - Out of stock
- `GET /api/products/category/{categoryId}` - By category
- `GET /api/products/{id}` - By ID
- `GET /api/products/sku/{sku}` - By SKU
- `POST /api/products` - Create
- `PUT /api/products/{id}` - Update
- `POST /api/products/{id}/adjust-stock` - Adjust stock
- `DELETE /api/products/{id}` - Delete

**Categories (6 endpoints):**
- `GET /api/categories` - All categories
- `GET /api/categories/active` - Active only
- `GET /api/categories/{id}` - By ID
- `POST /api/categories` - Create
- `PUT /api/categories/{id}` - Update
- `DELETE /api/categories/{id}` - Delete

### ✅ Repository Pattern & Unit of Work
- Generic repository for CRUD operations
- Specialized repositories for Products and Categories
- Unit of Work for managing multiple repositories
- Transaction support (BeginTransaction, Commit, Rollback)
- Lazy initialization of repositories

### ✅ Domain Logic (Product Aggregate)
- Stock management (reduce/increase with validation)
- Price management
- Out-of-stock detection
- Activation/deactivation
- Business rule validation

### ✅ Dependency Injection
- Centralized service registration
- Scoped services for DbContext and Unit of Work
- Interface-based abstractions
- Automatic logging integration

### ✅ Swagger/OpenAPI
- Full API documentation
- Request/response models
- HTTP status code descriptions
- Try-it-out functionality

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- SQL Server (LocalDB, Express, or Full Edition)
- Visual Studio Code or Visual Studio 2022

### Installation

1. **Restore NuGet packages:**
   ```bash
   cd d:\VSCode\ApiManagement
   dotnet restore
   ```

2. **Build the solution:**
   ```bash
   dotnet build
   ```

3. **Create the database:**
   ```bash
   cd src\Api
   dotnet ef database update
   ```

4. **Run the application:**
   ```bash
   dotnet run
   ```

Application runs on:
- HTTPS: `https://localhost:7001`
- HTTP: `http://localhost:5001`
- Swagger UI: `https://localhost:7001`

## 📚 API Usage Examples

### Create Category
```bash
POST /api/categories
{
  "name": "Electronics",
  "description": "Electronic devices and accessories"
}
```

### Create Product
```bash
POST /api/products
{
  "name": "Laptop",
  "description": "High-performance laptop",
  "sku": "LAPTOP-001",
  "price": 999.99,
  "categoryId": 1,
  "quantityInStock": 50
}
```

### Get Products by Category
```bash
GET /api/products/category/1
```

### Adjust Product Stock
```bash
POST /api/products/1/adjust-stock
{
  "quantity": -5,
  "reason": "Sale"
}
```

## 🗄️ Database Schema

### Products Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK |
| Name | varchar(255) | Required |
| Sku | varchar(50) | Unique, Required |
| Description | varchar(2000) | |
| Price | decimal(18,2) | Required |
| CategoryId | int | FK, Required |
| QuantityInStock | int | Default 0 |
| IsActive | bit | Default 1 |
| ImageUrl | varchar(500) | |
| CreatedAt | datetime | Default GETUTCDATE() |
| UpdatedAt | datetime | |
| IsDeleted | bit | Default 0 |

### Categories Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK |
| Name | varchar(255) | Unique, Required |
| Description | varchar(500) | |
| IsActive | bit | Default 1 |
| CreatedAt | datetime | Default GETUTCDATE() |
| UpdatedAt | datetime | |
| IsDeleted | bit | Default 0 |

## 🎯 Domain Logic Examples

### Product Stock Management
```csharp
// Reducing stock with validation
product.ReduceStock(quantity);  // Throws if insufficient

// Increasing stock
product.IncreaseStock(quantity);

// Check stock status
bool outOfStock = product.IsOutOfStock();
```

### Price Management
```csharp
// Update with validation
product.UpdatePrice(newPrice);  // Throws if negative
```

## 🛡️ Exception Handling

### Validation Error (400)
```json
{
  "message": "Product name is required",
  "code": "VALIDATION_ERROR",
  "timestamp": "2024-03-16T10:30:00Z"
}
```

### Not Found Error (404)
```json
{
  "message": "Product with id '999' not found",
  "code": "ENTITY_NOT_FOUND",
  "timestamp": "2024-03-16T10:31:00Z"
}
```

### Business Rule Error (422)
```json
{
  "message": "Insufficient stock. Requested: 100, Available: 50",
  "code": "INSUFFICIENT_STOCK",
  "timestamp": "2024-03-16T10:32:00Z"
}
```

### Duplicate Error (422)
```json
{
  "message": "Product SKU with 'LAPTOP-001' already exists",
  "code": "DUPLICATE_ENTITY",
  "timestamp": "2024-03-16T10:33:00Z"
}
```

## 📊 Response Format

### Success Response
```json
{
  "success": true,
  "data": { /* entity data */ },
  "message": "Operation successful",
  "timestamp": "2024-03-16T10:30:00Z"
}
```

### Error Response
```json
{
  "message": "Error message",
  "code": "ERROR_CODE",
  "timestamp": "2024-03-16T10:30:00Z",
  "traceId": "0HN5JMPL..."
}
```

## 🔧 Configuration

### Database Connection
Edit `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ProductManagementDb;..."
}
```

### Logging
Configure log levels:
```json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.EntityFrameworkCore": "Information"
  }
}
```

## 📦 Dependencies

- Microsoft.EntityFrameworkCore (8.0.0)
- Microsoft.EntityFrameworkCore.SqlServer (8.0.0)
- Swashbuckle.AspNetCore (6.4.6)
- .NET 8.0

## 🚀 Deployment

### Build for Production
```bash
dotnet publish -c Release -o ./publish
```

### Docker Support
Create a Dockerfile for containerization (not included in base project)

## 📋 Best Practices Implemented

✅ SOLID Principles
✅ DDD Patterns
✅ Repository Pattern
✅ Unit of Work Pattern
✅ Dependency Injection
✅ Async/Await
✅ Input Validation
✅ Business Rule Enforcement
✅ Logging & Monitoring
✅ Exception Handling
✅ RESTful API Design
✅ Swagger Documentation

## 🔐 Security Considerations

Implemented:
- ✅ CORS policy
- ✅ Input validation
- ✅ SQL injection prevention (EF Core)
- ✅ Error handling (no stack traces exposed)
- ✅ HTTPS enforcement

Recommended:
- [ ] JWT Authentication
- [ ] Authorization policies
- [ ] Rate limiting
- [ ] API versioning
- [ ] Request logging

## 📖 Documentation

- **README.md** - This file
- **SETUP.md** - Setup and deployment guide
- **DEVELOPMENT.md** - Development patterns
- **QUICK_REFERENCE.md** - Common tasks

## 🤝 Support

For issues or questions:
1. Check the SETUP.md for common problems
2. Review DEVELOPMENT.md for patterns
3. Check error codes in exception handling

## 📄 License

Open source - Use freely for personal and commercial projects.

---

**Created:** March 16, 2026
**Version:** 1.0
**Framework:** .NET 8.0
**Architecture:** Domain-Driven Design (DDD)
