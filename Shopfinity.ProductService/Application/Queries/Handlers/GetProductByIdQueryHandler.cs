using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopfinity.ProductService.Application.DTOs;
using Shopfinity.ProductService.Infrastructure;

namespace Shopfinity.ProductService.Application.Queries.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDTO>
    {
        private readonly ProductDbContext _dbContext;

        public GetProductByIdQueryHandler(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the product from the database
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            // If the product is not found, return null
            if (product == null)
            {
                return null;
            }

            var productDTO = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity,
                Active = product.Active,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };

            return productDTO;
        }
    }
}
