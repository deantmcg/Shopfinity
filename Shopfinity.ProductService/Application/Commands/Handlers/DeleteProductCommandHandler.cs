using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopfinity.ProductService.Infrastructure;

namespace Shopfinity.ProductService.Application.Commands.Handlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly ProductDbContext _dbContext;

        public DeleteProductCommandHandler(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            // Find the product by ID
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                // Product does not exist
                return false;
            }

            // Remove the product from the database
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
