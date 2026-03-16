# Quick Start Guide - Product Management API

## Prerequisites

- **.NET SDK 8.0** (Download from https://dotnet.microsoft.com/download)
- **SQL Server LocalDB** (Installed with Visual Studio or Docker Desktop)
- **Visual Studio Code** or **Visual Studio 2022+**

Verify installation:
```bash
dotnet --version
```

---

## 🚀 Quick Start (5 minutes)

### 1. Clone/Navigate to Project
```bash
cd d:\VSCode\ApiManagement
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Build Solution
```bash
dotnet build
```

Expected output:
```
Build succeeded with 2 warning(s)
```

### 4. Create Database
```bash
cd src/Api
dotnet ef database update
```

### 5. Start the Application
```bash
dotnet run
```

**Expected output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7001
info: Microsoft.Hosting.Lifetime[0]
      Application started
```

### 6. Open Swagger UI
Navigate to: **https://localhost:7001**

---

## 📋 Common Commands

### Clean Build
```bash
dotnet clean && dotnet build
```

### Run Tests
```bash
dotnet test
```

### Create Database Migration
```bash
cd src/Api
dotnet ef migrations add <MigrationName>
```

### Update Database with Latest Migration
```bash
cd src/Api
dotnet ef database update
```

### Publish Release Build
```bash
dotnet publish -c Release -o ./publish
```

---

## 🧪 Example API Calls

### 1. Create a Category
```bash
curl -X POST https://localhost:7001/api/categories \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Electronics",
    "description": "Electronic products"
  }'
```

**Response (201 Created):**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "name": "Electronics",
    "description": "Electronic products",
    "isActive": true,
    "productCount": 0,
    "createdAt": "2024-01-15T10:30:45.123Z"
  },
  "message": "Category created successfully",
  "timestamp": "2024-01-15T10:30:45.123Z"
}
```

### 2. Create a Product
```bash
curl -X POST https://localhost:7001/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Laptop Pro 15",
    "description": "High-performance 15-inch laptop",
    "sku": "LAPTOP-001",
    "price": 1299.99,
    "categoryId": 1,
    "quantityInStock": 50,
    "imageUrl": "https://example.com/laptop.jpg"
  }'
```

**Response (201 Created):**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "name": "Laptop Pro 15",
    "sku": "LAPTOP-001",
    "price": 1299.99,
    "categoryId": 1,
    "categoryName": "Electronics",
    "quantityInStock": 50,
    "isActive": true,
    "isOutOfStock": false,
    "createdAt": "2024-01-15T10:31:00.123Z"
  },
  "message": "Product created successfully",
  "timestamp": "2024-01-15T10:31:00.123Z"
}
```

### 3. Get All Products
```bash
curl https://localhost:7001/api/products
```

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "Laptop Pro 15",
      "sku": "LAPTOP-001",
      "price": 1299.99,
      "categoryId": 1,
      "categoryName": "Electronics",
      "quantityInStock": 50,
      "isActive": true,
      "isOutOfStock": false,
      "createdAt": "2024-01-15T10:31:00.123Z"
    }
  ],
  "message": "Products retrieved successfully",
  "timestamp": "2024-01-15T10:31:05.123Z"
}
```

### 4. Get Active Products
```bash
curl https://localhost:7001/api/products/active
```

### 5. Get Product by SKU
```bash
curl https://localhost:7001/api/products/sku/LAPTOP-001
```

### 6. Get Out of Stock Products
```bash
curl https://localhost:7001/api/products/out-of-stock
```

### 7. Adjust Product Stock
```bash
curl -X POST https://localhost:7001/api/products/1/adjust-stock \
  -H "Content-Type: application/json" \
  -d '{
    "productId": 1,
    "quantity": -5,
    "reason": "Sold to customer"
  }'
```

### 8. Update Product Price
```bash
curl -X PUT https://localhost:7001/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "name": "Laptop Pro 15",
    "description": "High-performance 15-inch laptop",
    "price": 1199.99,
    "categoryId": 1,
    "imageUrl": "https://example.com/laptop.jpg"
  }'
```

### 9. Delete Product
```bash
curl -X DELETE https://localhost:7001/api/products/1
```

**Response (204 No Content)**

### 10. Health Check
```bash
curl https://localhost:7001/health
```

---

## 🔍 Troubleshooting

### Issue: "Could not connect to database"
**Solution:** Ensure LocalDB is running
```bash
sqllocaldb start mssqllocaldb
```

### Issue: "Port 7001 is already in use"
**Solution:** Stop the running instance or specify different port
```bash
dotnet run --urls "https://localhost:7002"
```

### Issue: "Entity Framework tools not found"
**Solution:** Install EF Core tools globally
```bash
dotnet tool install --global dotnet-ef
```

### Issue: Build fails with "ProduceResponseType not found"
**Solution:** Ensure Swashbuckle is installed (already done in this project)

---

## 📚 Project Structure

```
src/
├── Domain/              (Business logic, entities, interfaces)
├── Application/         (Services, DTOs, use cases)
├── Infrastructure/      (Database, repositories, migrations)
└── Api/                 (REST endpoints, middleware, configuration)
```

---

## 🔐 Security Notes

⚠️ **For Development Only:**
- Database connection uses Trusted Connection (Windows authentication)
- No authentication/authorization implemented yet
- CORS is set to "AllowAll" for development

🔒 **For Production:**
1. Implement JWT authentication
2. Add role-based access control
3. Restrict CORS to specific origins
4. Use connection string with SQL authentication
5. Enable HTTPS certificate validation
6. Add rate limiting
7. Implement request logging
8. Add API versioning

---

## 📖 API Documentation

Full Swagger documentation available at: **https://localhost:7001**

### Available Endpoints

**Products (10 endpoints)**
- `GET /api/products` - Get all
- `GET /api/products/active` - Get active only
- `GET /api/products/out-of-stock` - Get out of stock
- `GET /api/products/category/{categoryId}` - By category
- `GET /api/products/{id}` - By ID
- `GET /api/products/sku/{sku}` - By SKU
- `POST /api/products` - Create
- `PUT /api/products/{id}` - Update
- `POST /api/products/{id}/adjust-stock` - Adjust stock
- `DELETE /api/products/{id}` - Delete

**Categories (6 endpoints)**
- `GET /api/categories` - Get all
- `GET /api/categories/active` - Get active only
- `GET /api/categories/{id}` - By ID
- `POST /api/categories` - Create
- `PUT /api/categories/{id}` - Update
- `DELETE /api/categories/{id}` - Delete

---

## 🐛 Debugging

### Enable Detailed Logging

Modify `appsettings.Development.json`:
```json
"Logging": {
  "LogLevel": {
    "Default": "Debug",
    "Microsoft": "Information",
    "Microsoft.EntityFrameworkCore": "Debug"
  }
}
```

### Check Database Schema

Query the database directly:
```sql
SELECT * FROM [ProductManagementDb].dbo.[Products];
SELECT * FROM [ProductManagementDb].dbo.[Categories];
```

---

## 📞 Support

For issues or questions:
1. Check the [README.md](README.md) for detailed documentation
2. Review [BUILD_SUMMARY.md](BUILD_SUMMARY.md) for architecture overview
3. Check error responses from API endpoints (include traceId)

---

**Last Updated:** 2024
**Status:** ✅ Ready for Development
