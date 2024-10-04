using Shopfinity.ProductService.Application.Queries;
using Shopfinity.ProductService.Application.Queries.Handlers;
using Shopfinity.ProductService.Infrastructure;

namespace Shopfinity.ProductService.Tests.Handlers.Queries
{
    public class GetAllActiveProductsQueryHandlerTests : BaseTest
    {
        [Fact]
        public async Task Handle_ShouldReturnOnlyActiveProducts()
        {
            // Arrange
            _dbContext.Products.AddRange(
                new Product { Id = 1, Name = "Product1", Active = true },
                new Product { Id = 2, Name = "Product2", Active = false },
                new Product { Id = 3, Name = "Product3", Active = true }
            );
            await _dbContext.SaveChangesAsync();

            var handler = new GetAllActiveProductsQueryHandler(_dbContext);

            // Act
            var result = await handler.Handle(new GetAllActiveProductsQuery(), default);

            // Assert
            Assert.Equal(2, result.Count());
        }
    }
}