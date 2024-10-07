namespace Shopfinity.ProductService.Models
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryMinutes { get; set; } = 60 * 24 * 365; // 1 Year in minutes;
    }
}
