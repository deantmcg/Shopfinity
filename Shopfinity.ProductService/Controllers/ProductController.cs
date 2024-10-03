using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopfinity.ProductService.Application.DTOs;
using Shopfinity.ProductService.Application.Queries;

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
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        // GET: api/Product/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllActiveProducts()
        {
            var query = new GetAllActiveProductsQuery();
            var products = await _mediator.Send(query);

            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
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
    }
}