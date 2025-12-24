using Microsoft.EntityFrameworkCore;
using MilkProductsCatalog.Models;
using MilkProducts.Shared.Configuration;
using MilkProducts.Shared.Data;

namespace MilkProductsCatalog
{
    public static class DatabaseHelper
    {
        private const string DefaultFallback = "Server=.\\SQLEXPRESS;Database=Familia22i1L9;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;";

        public static string[] GetConnectionStrings()
        {
            var primary = ConnectionStringProvider.GetConnectionString(DefaultFallback);

            var fallbacks = new[]
            {
                primary,
                "Server=(localdb)\\MSSQLLocalDB;Database=Familia22i1L9;Trusted_Connection=true;TrustServerCertificate=true;Encrypt=false;",
                "Server=.;Database=Familia22i1L9;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;",
                "Server=.\\MSSQLSERVER;Database=Familia22i1L9;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;",
                "Server=localhost;Database=Familia22i1L9;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;"
            };

            return fallbacks.Distinct().ToArray();
        }

        public static (bool success, string workingConnection, string error) TestConnections()
        {
            foreach (var connectionString in GetConnectionStrings())
            {
                try
                {
                    var options = new DbContextOptionsBuilder<SalesContext>();
                    DbContextOptionsFactory.ConfigureSqlServer(options, connectionString);

                    using var context = new SalesContext(options.Options);

                    if (context.Database.CanConnect())
                    {
                        return (true, connectionString, string.Empty);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return (false, string.Empty, "Не удалось подключиться ни к одному экземпляру SQL Server. Проверьте ConnectionStrings__SqlServer и доступность сервиса.");
        }

        public static bool CreateDatabaseIfNotExists(string connectionString)
        {
            try
            {
                var options = new DbContextOptionsBuilder<SalesContext>();
                DbContextOptionsFactory.ConfigureSqlServer(options, connectionString);

                using var context = new SalesContext(options.Options);
                return context.Database.EnsureCreated();
            }
            catch
            {
                return false;
            }
        }
    }
}
