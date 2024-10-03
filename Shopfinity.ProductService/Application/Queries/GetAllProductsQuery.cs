using MediatR;
using Shopfinity.ProductService.Application.DTOs;

namespace Shopfinity.ProductService.Application.Queries
{
    // Query to get all products
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDTO>>
    {
    }
}
