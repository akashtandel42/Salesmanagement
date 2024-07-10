using Microsoft.EntityFrameworkCore;
using Salesmanagement.Models;

namespace Salesmanagement.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<Product> Products { get; set; }

        // Add DbSet properties for other entities as needed

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships, constraints, seed data, etc.
            base.OnModelCreating(modelBuilder);
        }
    }
}
