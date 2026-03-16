using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddLogging();

var app = builder.Build();

// Use middleware pipeline
app.UseApplicationMiddleware(app.Environment);

// Initialize database
await app.InitializeDatabaseAsync();

app.Run();
