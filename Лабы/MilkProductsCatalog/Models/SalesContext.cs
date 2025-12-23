using Microsoft.EntityFrameworkCore;

namespace MilkProductsCatalog.Models
{
    public partial class SalesContext : DbContext
    {
        private static string? _workingConnectionString;

        public SalesContext()
        {
        }

        public SalesContext(DbContextOptions<SalesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Sale> Sales { get; set; } = null!;
        public virtual DbSet<SaleDetails> SaleDetails { get; set; } = null!;

        public static void SetWorkingConnectionString(string connectionString)
        {
            _workingConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Если уже найдена рабочая строка подключения, используем её
                if (!string.IsNullOrEmpty(_workingConnectionString))
                {
                    optionsBuilder.UseSqlServer(_workingConnectionString);
                    return;
                }

                // Используем SQL Server Express по умолчанию без тестирования подключений
                // чтобы избежать циклических зависимостей при инициализации
                string defaultConnection = "Server=.\\SQLEXPRESS;Database=Familia22i1L9;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;";
                optionsBuilder.UseSqlServer(defaultConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.SaleId);
                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<SaleDetails>(entity =>
            {
                entity.HasKey(e => e.SaleDetailId);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SaleDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.SaleDetails)
                    .HasForeignKey(d => d.SaleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}