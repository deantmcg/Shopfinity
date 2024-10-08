using Shopfinity.ProductService.Application.Commands;
using Shopfinity.ProductService.Application.Commands.Handlers;

namespace Shopfinity.ProductService.Tests.Handlers.Commands
{
    public class DeleteProductCommandHandlerTests : BaseTest
    {
        [Fact]
        public async Task Handle_SuccessfullyDeleteProduct()
        {
            // Arrange
            var createCommand = new CreateProductCommand
            {
                Name = "New Product",
            };

            var createHandler = new CreateProductCommandHandler(_dbContext);
            var createResult = await createHandler.Handle(createCommand, default);

            // Verify the product was created
            var createdProduct = await _dbContext.Products.FindAsync(createResult);
            Assert.NotNull(createdProduct); // Ensure the product exists before deletion

            var deleteCommand = new DeleteProductCommand(createResult);
            var deleteHandler = new DeleteProductCommandHandler(_dbContext);

            // Act
            var deleteResult = await deleteHandler.Handle(deleteCommand, default);

            // Assert
            Assert.True(deleteResult); // Ensure deletion was successful

            // Verify the product no longer exists
            var deletedProduct = await _dbContext.Products.FindAsync(createResult);
            Assert.Null(deletedProduct); // Ensure the product has been deleted
        }


        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new DeleteProductCommand(1); // Product ID that does not exist

            var handler = new DeleteProductCommandHandler(_dbContext);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.False(result);
        }
    }
}