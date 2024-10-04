using Shopfinity.ProductService.Application.Commands;
using Shopfinity.ProductService.Application.Commands.Handlers;

namespace Shopfinity.ProductService.Tests.Handlers.Commands
{
    public class CreateProductCommandHandlerTests : BaseTest
    {
        [Fact]
        public async Task Handle_ShouldCreateProduct()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "New Product",
                Description = "New Product Description",
                Price = 99.99m,
                StockQuantity = 10,
                Active = true,
                ImageUrl = "http://example.com/product.jpg"
            };

            var handler = new CreateProductCommandHandler(_dbContext);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            var createdProduct = await _dbContext.Products.FindAsync(result);
            Assert.NotNull(createdProduct);
            Assert.Equal("New Product", createdProduct.Name);
        }
    }
}