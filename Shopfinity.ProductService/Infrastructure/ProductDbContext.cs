using Microsoft.EntityFrameworkCore;

namespace Shopfinity.ProductService.Infrastructure
{
    public class ProductDbContext : DbContext
    {
        // Constructor that takes DbContextOptions, which allows configuration of the context
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        // DbSet represents a table in the database
        public DbSet<Product> Products { get; set; }

        // Optional: OnModelCreating method to customize entity mappings (e.g., table names, keys, relationships)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API configuration if necessary (e.g., configure table names or relationships)
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);  // Example: product names should be required and up to 100 characters
        }
    }
}