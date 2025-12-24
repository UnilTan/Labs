using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Kislomolochnye_products
{
    public partial class MainWindow : Window
    {
        private SalesContext db = new SalesContext();

        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                // Проверяем подключение к базе данных
                if (!db.Database.CanConnect())
                {
                    ShowConnectionError();
                    return;
                }

                var products = db.Products.ToList();
                ProductsDataGrid.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowConnectionError()
        {
            MessageBox.Show(
                "Не удается подключиться к базе данных.\n\n" +
                "Проверьте:\n" +
                "1. Запущен ли SQL Server\n" +
                "2. Существует ли база данных Familia22i1L9\n" +
                "3. Правильность строки подключения\n\n" +
                "Нажмите 'Тест подключения' для диагностики.",
                "Ошибка подключения к БД",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private void BtnPrice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var products = db.Products.ToList();
                
                if (products.Any())
                {
                    var maxPrice = products.Max(p => p.Price);
                    var minPrice = products.Min(p => p.Price);
                    var avgPrice = products.Average(p => p.Price);

                    txtMaxPrice.Text = maxPrice.ToString("F2");
                    txtMinPrice.Text = minPrice.ToString("F2");
                    txtAvgPrice.Text = avgPrice.ToString("F2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка вычисления цен: {ex.Message}", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnSortAsc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sortedProducts = db.Products.OrderBy(p => p.Price).ToList();
                ProductsDataGrid.ItemsSource = sortedProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки: {ex.Message}", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSortDesc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sortedProducts = db.Products.OrderByDescending(p => p.Price).ToList();
                ProductsDataGrid.ItemsSource = sortedProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки: {ex.Message}", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadProducts();
                    return;
                }

                var foundProducts = db.Products
                    .Where(p => p.ProductName.Contains(searchTerm))
                    .ToList();

                ProductsDataGrid.ItemsSource = foundProducts;

                if (!foundProducts.Any())
                {
                    MessageBox.Show("Товары не найдены", "Поиск", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnPartialSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchTerm = txtPartialSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadProducts();
                    return;
                }

                var foundProducts = db.Products
                    .Where(p => p.ProductName.StartsWith(searchTerm))
                    .ToList();

                ProductsDataGrid.ItemsSource = foundProducts;

                if (!foundProducts.Any())
                {
                    MessageBox.Show("Товары не найдены", "Поиск", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnSales_Click(object sender, RoutedEventArgs e)
        {
            var salesWindow = new SalesWindow();
            salesWindow.Show();
        }

        private void BtnSaleDetails_Click(object sender, RoutedEventArgs e)
        {
            var saleDetailsWindow = new SaleDetailsWindow();
            saleDetailsWindow.Show();
        }

        private void BtnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            var testWindow = new ConnectionTestWindow();
            testWindow.ShowDialog();
        }

        protected override void OnClosed(EventArgs e)
        {
            db?.Dispose();
            base.OnClosed(e);
        }

        // Обработчики событий для привязки к кнопкам в XAML
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Привязываем обработчики событий к кнопкам
            btnPrice.Click += BtnPrice_Click;
            btnSortAsc.Click += BtnSortAsc_Click;
            btnSortDesc.Click += BtnSortDesc_Click;
            btnSearch.Click += BtnSearch_Click;
            btnPartialSearch.Click += BtnPartialSearch_Click;
            btnSales.Click += BtnSales_Click;
            btnSaleDetails.Click += BtnSaleDetails_Click;
            btnTestConnection.Click += BtnTestConnection_Click;
        }
    }

    // Модели данных (если они не определены в отдельных файлах)
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public int ExpiryDays { get; set; }
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<SaleDetails> SaleDetails { get; set; } = new List<SaleDetails>();
    }

    public class Sale
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public virtual ICollection<SaleDetails> SaleDetails { get; set; } = new List<SaleDetails>();
    }

    public class SaleDetails
    {
        public int SaleDetailId { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual Sale Sale { get; set; } = null!;
    }
    // Контекст базы данных
    public class SalesContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Sale> Sales { get; set; } = null!;
        public virtual DbSet<SaleDetails> SaleDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Строка подключения для SQL Server Express
                string connectionString = "Server=.\\SQLEXPRESS;Database=Familia22i1L9;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;";
                optionsBuilder.UseSqlServer(connectionString);
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
        }
    }
}