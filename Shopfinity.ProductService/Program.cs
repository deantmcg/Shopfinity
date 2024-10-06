using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopfinity.ProductService.Infrastructure;
using Shopfinity.ProductService.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

await InitializeSeedDataAsync(app);

ConfigureMiddleware(app);

app.Run();


// Add and configure services
void ConfigureServices(WebApplicationBuilder builder)
{
    // Add Controllers
    builder.Services.AddControllers();

    // Configure DbContext for Product Service
    ConfigureProductDbContext(builder);

    // Configure Identity for User Management
    ConfigureIdentity(builder);

    // Add MediatR for CQRS
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    });

    // Swagger Configuration for API Documentation
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

// Configure ProductDbContext
void ConfigureProductDbContext(WebApplicationBuilder builder)
{
    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddDbContext<ProductDbContext>(options =>
            options.UseInMemoryDatabase("InMemoryProductDb"));
    }
    else
    {
        builder.Services.AddDbContext<ProductDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    }
}

// Configure Identity for User Management
void ConfigureIdentity(WebApplicationBuilder builder)
{
    if (builder.Environment.IsDevelopment())
    {
        // Configure local ASP.NET Core Identity for dev
        builder.Services.AddDbContext<UserDbContext>(options =>
            options.UseInMemoryDatabase("InMemoryUserDb"));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();
    }
    else
    {
        // Configure AWS Cognito for production
        // TODO: Implement AWS Cognito configuration
        // builder.Services.AddAuthentication(...).AddJwtBearer(...);
    }
}

async Task InitializeSeedDataAsync(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            // Seed Product Data
            var productDbContext = services.GetRequiredService<ProductDbContext>();
            SeedProductData.Initialize(productDbContext);

            // Seed User Data
            await SeedUserData.InitializeAsync(services);
        }
        catch (Exception ex)
        {
            // Log any errors encountered during seed initialization
            Console.WriteLine($"Error seeding database: {ex.Message}");
        }
    }
}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}