using Microsoft.Extensions.Configuration;

namespace MilkProducts.Shared.Configuration
{
    public static class ConnectionStringProvider
    {
        private const string DefaultKey = "SqlServer";
        private static string? _cached;

        public static string GetConnectionString(string? fallback = null)
        {
            if (!string.IsNullOrWhiteSpace(_cached))
            {
                return _cached!;
            }

            var configuration = BuildConfiguration();
            var fromConfig = configuration.GetConnectionString(DefaultKey);

            var resolved = !string.IsNullOrWhiteSpace(fromConfig)
                ? fromConfig
                : fallback;

            if (string.IsNullOrWhiteSpace(resolved))
            {
                throw new InvalidOperationException(
                    "Connection string 'SqlServer' is not configured. Set ConnectionStrings:SqlServer in appsettings.json or environment variable ConnectionStrings__SqlServer.");
            }

            _cached = resolved;
            return resolved!;
        }

        public static void ResetCache() => _cached = null;

        public static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
