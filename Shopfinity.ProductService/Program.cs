using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shopfinity.ProductService.Constants;
using Shopfinity.ProductService.Infrastructure;
using Shopfinity.ProductService.Interfaces;
using Shopfinity.ProductService.Models;
using Shopfinity.ProductService.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

await InitializeSeedDataAsync(app);

ConfigureMiddleware(app);

app.Run();


// Add and configure services
void ConfigureServices(WebApplicationBuilder builder)
{
    // Read JWT Settings
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

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

    // Register Token Service
    builder.Services.AddScoped<ITokenService, TokenService>();

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

        // Configure JWT authentication
        var key = builder.Configuration[Constants.ConfigJwtKey];
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration[Constants.ConfigJwtIssuer],
                ValidAudience = builder.Configuration[Constants.ConfigJwtAudience],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        });
    }
    else
    {
        // Configure AWS Cognito for production
        // TODO: Implement AWS Cognito configuration
        // builder.Services.AddAuthentication(...).AddJwtBearer(...);
    }

    // Configure Authentication
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };

        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    });
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
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
}