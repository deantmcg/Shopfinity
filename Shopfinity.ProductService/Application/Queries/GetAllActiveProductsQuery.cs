using MediatR;
using Shopfinity.ProductService.Application.DTOs;

namespace Shopfinity.ProductService.Application.Queries
{
    // Query to get all active products
    public class GetAllActiveProductsQuery : IRequest<IEnumerable<ProductDTO>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}