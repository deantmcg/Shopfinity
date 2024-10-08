using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopfinity.ProductService.Application.DTOs;
using Shopfinity.ProductService.Infrastructure;

namespace Shopfinity.ProductService.Application.Queries.Handlers
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>
    {
        private readonly ProductDbContext _context;

        public GetAllProductsQueryHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            // Fetch all products from the database
            var products = await _context.Products.ToListAsync(cancellationToken);

            var productDTOs = products.Select(product => new ProductDTO
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
            });

            return productDTOs.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize); ;
        }
    }
}
