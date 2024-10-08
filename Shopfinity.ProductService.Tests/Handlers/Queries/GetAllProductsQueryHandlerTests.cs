using Shopfinity.ProductService.Application.Queries;
using Shopfinity.ProductService.Application.Queries.Handlers;
using Shopfinity.ProductService.Infrastructure;
using Shopfinity.ProductService.Tests.Handlers;

namespace Shopfinity.ProductService.Tests.Handlers.Queries
{
    public class GetAllProductsQueryHandlerTests : BaseTest
    {
        [Fact]
        public async Task Handle_ShouldReturnAllProducts()
        {
            // Arrange
            _dbContext.Products.AddRange(new Product { Id = 1, Name = "Product1", Active = true },
                                        new Product { Id = 2, Name = "Product2", Active = true });
            await _dbContext.SaveChangesAsync();

            var handler = new GetAllProductsQueryHandler(_dbContext);

            // Act
            var result = await handler.Handle(new GetAllProductsQuery(), default);

            // Assert
            Assert.Equal(2, result.Count());
        }
    }
}