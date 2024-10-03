using MediatR;
using Shopfinity.ProductService.Application.DTOs;

namespace Shopfinity.ProductService.Application.Queries
{
    public class GetProductByIdQuery : IRequest<ProductDTO>
    {
        public int Id { get; set; } // ID of the product to retrieve

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}