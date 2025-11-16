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
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<ProductExtended>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<ProductExtended>()
                .Property(p => p.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<ProductExtended>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            modelBuilder.Entity<ProductExtended>()
                .Property(p => p.MinPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ProductExtended>()
                .Property(p => p.CreatedOn)
                .HasColumnType("datetime2");

            modelBuilder.Entity<ProductExtended>()
                .Property(p => p.UpdatedOn)
                .HasColumnType("datetime2");
        }
    }
}