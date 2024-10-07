using Shopfinity.ProductService.Models;

namespace Shopfinity.ProductService.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
