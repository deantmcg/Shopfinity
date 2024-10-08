using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shopfinity.ProductService.API.Controllers;
using Shopfinity.ProductService.Application.Commands;
using Shopfinity.ProductService.Application.DTOs;
using Shopfinity.ProductService.Application.Queries;

public class ProductControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        var _loggerMock = new Mock<ILogger<ProductController>>();
        _controller = new ProductController(_mediatorMock.Object, _loggerMock.Object);
    }

    // Test for Get All Products
    [Fact]
    public async Task GetAllProducts_ReturnsOkResult_WithListOfProducts()
    {
        // Arrange
        var products = new List<ProductDTO>
        {
            new ProductDTO { Id = 1, Name = "Product 1" },
            new ProductDTO { Id = 2, Name = "Product 2" }
        };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(products);

        // Act
        var result = await _controller.GetAllProducts();

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<ProductDTO>>>(result);
        var returnedProducts = Assert.IsType<OkObjectResult>(okResult.Result);
        var productsValue = Assert.IsType<List<ProductDTO>>(returnedProducts.Value);
        Assert.Equal(2, productsValue.Count);
    }

    // Test for Get Product by Id
    [Fact]
    public async Task GetProductById_ValidId_ReturnsOkResult_WithProduct()
    {
        // Arrange
        var product = new ProductDTO { Id = 1, Name = "Product 1" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(product);

        // Act
        var result = await _controller.GetProductById(1);

        // Assert
        var okResult = Assert.IsType<ActionResult<ProductDTO>>(result);
        var returnedProduct = Assert.IsType<OkObjectResult>(okResult.Result);
        var productValue = Assert.IsType<ProductDTO>(returnedProduct.Value);
        Assert.Equal(1, productValue.Id);
    }

    // Test for Create Product
    [Fact]
    public async Task CreateProduct_ValidCommand_ReturnsCreatedAtAction()
    {
        // Arrange
        var command = new CreateProductCommand { Name = "New Product", Price = 10.0M };
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(1);  // Assume the new product ID is 1

        // Act
        var result = await _controller.CreateProduct(command);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
    }

    // Test for Update Product
    [Fact]
    public async Task UpdateProduct_ValidCommand_ReturnsNoContent()
    {
        // Arrange
        var command = new UpdateProductCommand { Id = 1, Name = "Updated Product", Price = 15.0M };
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true); // Assume update was successful

        // Act
        var result = await _controller.UpdateProduct(1, command);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    // Test for Delete Product
    [Fact]
    public async Task DeleteProduct_ValidId_ReturnsNoContent()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true); // Assume deletion was successful

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
