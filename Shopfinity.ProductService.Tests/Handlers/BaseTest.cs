using Microsoft.EntityFrameworkCore;
using Shopfinity.ProductService.Infrastructure;

namespace Shopfinity.ProductService.Tests.Handlers
{
    public class BaseTest : IDisposable
    {
        protected readonly ProductDbContext _dbContext;
        protected readonly DbContextOptions<ProductDbContext> _options;

        public BaseTest()
        {
            _options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database for each test
                .Options;

            _dbContext = new ProductDbContext(_options);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted(); // Clean up the database after each test
            _dbContext.Dispose();
        }
    }
}