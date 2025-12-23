using Microsoft.EntityFrameworkCore;
using MilkProductsCatalog.Models;

namespace MilkProductsCatalog
{
    public static class DatabaseHelper
    {
        public static string[] GetConnectionStrings()
        {
            return new string[]
            {
                // SQL Server Express (наиболее распространенный)
                "Server=.\\SQLEXPRESS;Database=Familia22i1L9;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;",
                
                // LocalDB
                "Server=(localdb)\\mssqllocaldb;Database=Familia22i1L9;Integrated Security=true;TrustServerCertificate=true;",
                
                // SQL Server (стандартный)
                "Server=.;Database=Familia22i1L9;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;",
                
                // SQL Server с другим именем экземпляра
                "Server=.\\MSSQLSERVER;Database=Familia22i1L9;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;",
                
                // Localhost
                "Server=localhost;Database=Familia22i1L9;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;"
            };
        }

        public static (bool success, string workingConnection, string error) TestConnections()
        {
            var connectionStrings = GetConnectionStrings();
            
            foreach (var connectionString in connectionStrings)
            {
                try
                {
                    var options = new DbContextOptionsBuilder<SalesContext>()
                        .UseSqlServer(connectionString)
                        .Options;

                    using var context = new SalesContext(options);
                    
                    if (context.Database.CanConnect())
                    {
                        return (true, connectionString, string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    // Продолжаем проверку следующей строки подключения
                    continue;
                }
            }

            return (false, string.Empty, "Ни одна из стандартных строк подключения не работает");
        }

        public static bool CreateDatabaseIfNotExists(string connectionString)
        {
            try
            {
                var options = new DbContextOptionsBuilder<SalesContext>()
                    .UseSqlServer(connectionString)
                    .Options;

                using var context = new SalesContext(options);
                return context.Database.EnsureCreated();
            }
            catch
            {
                return false;
            }
        }
    }
}