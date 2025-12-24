using Microsoft.EntityFrameworkCore;

namespace MilkProductsBinding.Models
{
    public partial class SalesContext : DbContext
    {
        public SalesContext()
        {
        }

        public SalesContext(DbContextOptions<SalesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Product { get; set; } = null!;
        public virtual DbSet<Sale> Sale { get; set; } = null!;
        public virtual DbSet<SaleDetails> DetailSale { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = MilkProducts.Shared.Configuration.ConnectionStringProvider.GetConnectionString(
                    "Server=localhost;Database=Familia22i1L9;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;");

                MilkProducts.Shared.Data.DbContextOptionsFactory.ConfigureSqlServer(
                    optionsBuilder,
                    connectionString,
                    MilkProducts.Shared.Data.DbContextOptionsFactory.CreateLoggerFactory());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.idProduct);
                entity.Property(e => e.idProduct).HasColumnName("ProductId");
                entity.Property(e => e.nameProduct).HasColumnName("ProductName").IsRequired().HasMaxLength(100);
                entity.Property(e => e.priceProduct).HasColumnName("Price").HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.IdSale);
                entity.Property(e => e.IdSale).HasColumnName("SaleId");
                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
                
                // Новые поля для ЛР-13
                entity.Property(e => e.SummaSale).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.DiscountSale).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.Resultat).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<SaleDetails>(entity =>
            {
                entity.HasKey(e => e.IdDetailSale);
                entity.Property(e => e.IdDetailSale).HasColumnName("SaleDetailId");
                entity.Property(e => e.IdSale).HasColumnName("SaleId");
                entity.Property(e => e.IdProductDetailSale).HasColumnName("ProductId");
                entity.Property(e => e.QuantityProduct).HasColumnName("Quantity");
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
                
                // Новое поле для ЛР-13
                entity.Property(e => e.SummaProduct).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SaleDetails)
                    .HasForeignKey(d => d.IdProductDetailSale)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.SaleDetails)
                    .HasForeignKey(d => d.IdSale)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
