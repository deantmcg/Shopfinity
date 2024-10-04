using Shopfinity.ProductService.Infrastructure;

namespace Shopfinity.ProductService.Domain.Tests
{
    public static class TestProductFactory
    {
        public static List<Product> CreateTestProducts(int count, string baseName, decimal price, bool active)
        {
            var products = new List<Product>();

            for (int i = 1; i <= count; i++)
            {
                products.Add(new Product
                {
                    Id = i,
                    Name = $"{baseName} {i}",
                    Description = $"Description for {baseName} {i}",
                    Price = price,
                    StockQuantity = 10,
                    Active = active,
                    ImageUrl = $"http://example.com/product_{count}.jpg",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            return products;
        }
    }
}
