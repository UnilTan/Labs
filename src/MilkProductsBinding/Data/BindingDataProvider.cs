using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MilkProductsBinding.Models;
using MilkProducts.Shared.Configuration;
using MilkProducts.Shared.Data;

namespace MilkProductsBinding.Data
{
    /// <summary>
    /// Централизованная загрузка данных в ObservableCollection для привязок WPF.
    /// </summary>
    public class BindingDataProvider
    {
        private readonly DbContextOptions<SalesContext> _options;

        public ObservableCollection<Product> Products { get; } = new();
        public ObservableCollection<SaleDetails> SaleDetails { get; } = new();

        public BindingDataProvider(string? fallbackConnection = null)
        {
            var conn = ConnectionStringProvider.GetConnectionString(fallbackConnection);
            var builder = new DbContextOptionsBuilder<SalesContext>();
            DbContextOptionsFactory.ConfigureSqlServer(builder, conn);
            _options = builder.Options;
        }

        public void Reload()
        {
            Products.Clear();
            SaleDetails.Clear();

            using var context = new SalesContext(_options);
            foreach (var product in context.Product.AsNoTracking().ToList())
            {
                Products.Add(product);
            }

            foreach (var detail in context.DetailSale
                         .Include(d => d.Product)
                         .AsNoTracking()
                         .ToList())
            {
                SaleDetails.Add(detail);
            }
        }
    }
}
