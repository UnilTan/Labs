using Microsoft.EntityFrameworkCore;
using MilkProducts.Shared.Configuration;
using MilkProducts.Shared.Data;
using MilkProductsCatalog.Models;

namespace MilkProductsCatalog.Services
{
    /// <summary>
    /// Обертка над SalesContext для безопасной работы из форм.
    /// </summary>
    public class CatalogDataService
    {
        private readonly DbContextOptions<SalesContext> _options;

        public CatalogDataService(string? fallbackConnection = null)
        {
            var conn = ConnectionStringProvider.GetConnectionString(fallbackConnection);
            var builder = new DbContextOptionsBuilder<SalesContext>();
            DbContextOptionsFactory.ConfigureSqlServer(builder, conn);
            _options = builder.Options;
        }

        public List<Product> GetProducts()
        {
            using var context = new SalesContext(_options);
            return context.Products.AsNoTracking().ToList();
        }

        public (decimal max, decimal min, decimal avg) GetPriceStats()
        {
            using var context = new SalesContext(_options);
            var products = context.Products.AsNoTracking().ToList();
            if (!products.Any())
            {
                return (0, 0, 0);
            }

            return (products.Max(p => p.Price), products.Min(p => p.Price), products.Average(p => p.Price));
        }

        public bool TryConnect()
        {
            using var context = new SalesContext(_options);
            return context.Database.CanConnect();
        }
    }
}
