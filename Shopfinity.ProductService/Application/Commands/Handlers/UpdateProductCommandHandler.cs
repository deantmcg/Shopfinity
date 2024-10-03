using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopfinity.ProductService.Infrastructure;

namespace Shopfinity.ProductService.Application.Commands.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly ProductDbContext _dbContext;

        public UpdateProductCommandHandler(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            // Find the product by ID
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            // Product does not exist
            if (product == null)
            {
                return false;
            }

            // Update the product properties
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.ImageUrl = request.ImageUrl;
            product.StockQuantity = request.StockQuantity;
            product.Active = request.Active;
            product.UpdatedAt = DateTime.UtcNow;

            // Save changes to the database
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
