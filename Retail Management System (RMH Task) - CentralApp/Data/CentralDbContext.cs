using CentralApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CentralApp.Data
{
    public class CentralDbContext: DbContext
    {
        public CentralDbContext(DbContextOptions<CentralDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(500);
        }
    }
}