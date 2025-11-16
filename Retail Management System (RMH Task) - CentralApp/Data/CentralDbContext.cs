using CentralApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CentralApp.Data
{
    public class CentralDbContext: DbContext
    {
        public CentralDbContext(DbContextOptions<CentralDbContext> options)
            : base(options) { }

        public DbSet<ProductExtended> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductExtended>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<ProductExtended>()
                .Property(p => p.Description)
                .HasMaxLength(500);
        }
    }
}