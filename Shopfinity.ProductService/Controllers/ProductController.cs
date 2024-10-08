using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopfinity.ProductService.Application.Commands;
using Shopfinity.ProductService.Application.DTOs;
using Shopfinity.ProductService.Application.Queries;
using Shopfinity.ProductService.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Shopfinity.ProductService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMediator mediator, ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>List of all products.</returns>
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Gets all products", Description = "Retrieves a list of all available products")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            _logger.LogInformation("Fetching all products");
            var query = new GetAllProductsQuery();
            var products = await _mediator.Send(query);
            _logger.LogInformation("Products fetched successfully");
            return Ok(products);
        }

        /// <summary>
        /// Retrieves all active products.
        /// </summary>
        /// <returns>List of active products.</returns>
        [HttpGet("active")]
        [Authorize]
        [SwaggerOperation(Summary = "Gets all active products", Description = "Retrieves a list of products that are currently active")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllActiveProducts()
        {
            _logger.LogInformation("Fetching all active products");
            var query = new GetAllActiveProductsQuery();
            var products = await _mediator.Send(query);
            _logger.LogInformation("Fetched {Count} active products", products.Count());

            return Ok(products);
        }

        /// <summary>
        /// Retrieves a product by ID.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <returns>Product details.</returns>
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get product by ID", Description = "Retrieves details of a product by its ID")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            _logger.LogInformation("Fetching product with ID: {Id}", id);
            var query = new GetProductByIdQuery(id);
            var product = await _mediator.Send(query);

            if (product == null)
            {
                _logger.LogWarning("Product with ID: {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Fetched product with ID: {Id}", id);
            return Ok(product);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="command">Product creation command.</param>
        /// <returns>Created product details.</returns>
        [HttpPost]
        [Authorize(Roles = $"{UserRole.Admin},{UserRole.Manager}")]
        [SwaggerOperation(Summary = "Create a new product", Description = "Creates a new product in the system")]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            _logger.LogInformation("Creating a new product");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid product model state");
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(command);
            _logger.LogInformation("Created product with ID: {Id}", result);
            return CreatedAtAction(nameof(GetProductById), new { id = result }, result);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <param name="command">Product update command.</param>
        /// <returns>Status of the update operation.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRole.Admin},{UserRole.Manager}")]
        [SwaggerOperation(Summary = "Update product details", Description = "Updates the product details by ID")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand command)
        {
            _logger.LogInformation("Updating product with ID: {Id}", id);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid product model state for ID: {Id}", id);
                return BadRequest(ModelState);
            }

            if (id != command.Id)
            {
                _logger.LogWarning("Product ID mismatch: URL ID {Id} does not match body ID {BodyId}", id, command.Id);
                return BadRequest("Product ID in request body does not match URL.");
            }

            var result = await _mediator.Send(command);

            if (result == null)
            {
                _logger.LogWarning("Product with ID: {Id} not found for update", id);
                return NotFound();
            }

            _logger.LogInformation("Updated product with ID: {Id}", id);
            return NoContent();
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <returns>Status of the delete operation.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Admin)]
        [SwaggerOperation(Summary = "Delete a product", Description = "Deletes a product by ID")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation("Deleting product with ID: {Id}", id);
            var command = new DeleteProductCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
            {
                _logger.LogWarning("Product with ID: {Id} not found for deletion", id);
                return NotFound();
            }

            _logger.LogInformation("Deleted product with ID: {Id}", id);
            return NoContent();
        }
    }
}