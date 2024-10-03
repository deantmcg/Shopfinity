using MediatR;

namespace Shopfinity.ProductService.Application.Commands
{
    public class UpdateProductCommand : IRequest<bool>
    {
        // Identifies which product to update
        public int Id { get; set; }

        // Properties that can be updated
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public bool Active { get; set; }
    }
}
