using MediatR;
using Shopfinity.ProductService.Infrastructure;

namespace Shopfinity.ProductService.Application.Commands.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly ProductDbContext _dbContext;

        public CreateProductCommandHandler(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Map the request to a Product entity
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                StockQuantity = request.StockQuantity,
                Active = request.Active,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Add the product to the database
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Return the ID of the newly created product
            return product.Id;
        }
    }
}
