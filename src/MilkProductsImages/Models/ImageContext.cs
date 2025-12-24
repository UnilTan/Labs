using Microsoft.EntityFrameworkCore;

namespace MilkProductsImages.Models
{
    public class ImageContext : DbContext
    {
        public DbSet<ProductWithImage> ProductWithImage { get; set; }

                protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = MilkProducts.Shared.Configuration.ConnectionStringProvider.GetConnectionString(
                "Server=(localdb)\\MSSQLLocalDB;Database=Familia22i1L9;Trusted_Connection=true;TrustServerCertificate=true;");

            MilkProducts.Shared.Data.DbContextOptionsFactory.ConfigureSqlServer(
                optionsBuilder,
                connectionString,
                MilkProducts.Shared.Data.DbContextOptionsFactory.CreateLoggerFactory());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка модели ProductWithImage
            modelBuilder.Entity<ProductWithImage>(entity =>
            {
                entity.HasKey(e => e.IdProduct);
                
                entity.Property(e => e.NameProduct)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.PriceProduct)
                    .HasColumnType("decimal(10, 2)");
                
                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.Property(e => e.Description)
                    .HasMaxLength(255);
                
                // Настройка поля для изображения
                entity.Property(e => e.ProductImage)
                    .HasColumnType("varbinary(max)");
                
                entity.Property(e => e.ImageFileName)
                    .HasMaxLength(100);
                
                entity.Property(e => e.ImageContentType)
                    .HasMaxLength(50);
            });
        }
    }
}
