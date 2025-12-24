using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MilkProducts.Shared.Data
{
    public static class DbContextOptionsFactory
    {
        public static DbContextOptionsBuilder<TContext> ConfigureSqlServer<TContext>(
            DbContextOptionsBuilder<TContext> builder,
            string connectionString,
            ILoggerFactory? loggerFactory = null) where TContext : DbContext
        {
            builder.UseSqlServer(connectionString);
            builder.EnableDetailedErrors();
            builder.EnableSensitiveDataLogging(false);

            if (loggerFactory != null)
            {
                builder.UseLoggerFactory(loggerFactory);
            }

            return builder;
        }

        public static DbContextOptionsBuilder ConfigureSqlServer(
            DbContextOptionsBuilder builder,
            string connectionString,
            ILoggerFactory? loggerFactory = null)
        {
            builder.UseSqlServer(connectionString);
            builder.EnableDetailedErrors();
            builder.EnableSensitiveDataLogging(false);

            if (loggerFactory != null)
            {
                builder.UseLoggerFactory(loggerFactory);
            }

            return builder;
        }

        public static ILoggerFactory CreateLoggerFactory()
        {
            return LoggerFactory.Create(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Information);
                logging.AddDebug();
            });
        }
    }
}
