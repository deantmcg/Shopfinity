using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopfinity.ProductService.Application.DTOs;
using Shopfinity.ProductService.Infrastructure;

namespace Shopfinity.ProductService.Application.Queries.Handlers
{
    public class GetAllActiveProductsQueryHandler : IRequestHandler<GetAllActiveProductsQuery, IEnumerable<ProductDTO>>
    {
        private readonly ProductDbContext _dbContext;

        public GetAllActiveProductsQueryHandler(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetAllActiveProductsQuery request, CancellationToken cancellationToken)
        {
            // Retrieve all active products from the database
            var activeProducts = await _dbContext.Products
                .Where(p => p.Active)
                .ToListAsync(cancellationToken);

            var productDTOs = activeProducts.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                StockQuantity = p.StockQuantity,
                Active = p.Active,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList();

            return productDTOs;
        }
    }
}