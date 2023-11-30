using ConsumerDelivererAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsumerDelivererAPI.Infrastructure
{
    public class ProductOrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderProductDetails> OrderProductDetails { get; set; }

        public ProductOrderDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Kazemo mu da pronadje sve konfiguracije u Assembliju i da ih primeni nad bazom
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductOrderDbContext).Assembly);
        }
    }
}
