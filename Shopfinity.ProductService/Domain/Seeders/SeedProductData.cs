using Shopfinity.ProductService.Infrastructure;

public static class SeedProductData
{
    public static void Initialize(ProductDbContext context)
    {
        // Ensure the database is created and empty
        context.Database.EnsureCreated();

        // Check if there are any products already in the database
        if (context.Products.Any())
        {
            return; // DB has been seeded
        }

        // Add sample products
        var products = new List<Product>
        {
            new Product
            {
                Name = "Wireless Mouse",
                Description = "Ergonomic wireless mouse with adjustable DPI.",
                Price = 29.99m,
                ImageUrl = "https://example.com/images/wireless-mouse.jpg",
                StockQuantity = 100,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Mechanical Keyboard",
                Description = "RGB mechanical keyboard with customizable keys.",
                Price = 89.99m,
                ImageUrl = "https://example.com/images/mechanical-keyboard.jpg",
                StockQuantity = 50,
                Active = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "HD Webcam",
                Description = "1080p HD webcam for clear video calls.",
                Price = 49.99m,
                ImageUrl = "https://example.com/images/hd-webcam.jpg",
                StockQuantity = 75,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Laptop Stand",
                Description = "Adjustable laptop stand for ergonomic viewing.",
                Price = 39.99m,
                ImageUrl = "https://example.com/images/laptop-stand.jpg",
                StockQuantity = 200,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.Products.AddRange(products);
        context.SaveChanges();
    }
}