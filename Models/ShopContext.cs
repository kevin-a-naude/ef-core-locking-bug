using Microsoft.EntityFrameworkCore;

namespace EfCoreBugDemo.Models
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasData(
                    new Category()
                    {
                        Id = 1,
                        Name = "Footwear",
                        Discontinued = false,
                    });
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
