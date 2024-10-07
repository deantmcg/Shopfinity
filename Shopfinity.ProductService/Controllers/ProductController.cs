using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopfinity.ProductService.Application.Commands;
using Shopfinity.ProductService.Application.DTOs;
using Shopfinity.ProductService.Application.Queries;
using Shopfinity.ProductService.Constants;

namespace Shopfinity.ProductService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Product
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        // GET: api/Product/active
        [HttpGet("active")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllActiveProducts()
        {
            var query = new GetAllActiveProductsQuery();
            var products = await _mediator.Send(query);

            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var query = new GetProductByIdQuery(id);
            var product = await _mediator.Send(query);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        [Authorize(Roles = $"{UserRole.Admin},{UserRole.Manager}")]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = result }, result);
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRole.Admin},{UserRole.Manager}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != command.Id)
            {
                return BadRequest("Product ID in request body does not match URL.");
            }

            var result = await _mediator.Send(command);

            if (result == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var command = new DeleteProductCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}