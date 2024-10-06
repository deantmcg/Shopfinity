using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // For Entity Framework Core
using Shopfinity.ProductService.Infrastructure;
using Shopfinity.ProductService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add DbContext
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

// Configure MediatR
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    // Use AWS Cognito for production
    
    // Todo: implement AWS Cognito
    //builder.Services.AddAuthentication(...).AddJwtBearer(...);
}

var app = builder.Build();

// Seed the database with sample data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    SeedData.Initialize(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
