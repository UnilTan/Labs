using MilkProducts.Shared.Configuration;

namespace MilkProducts.Shared.Tests;

public class ConnectionStringProviderTests
{
    [Fact]
    public void ReturnsFallbackWhenNothingConfigured()
    {
        const string fallback = "Server=test;Database=Db;Trusted_Connection=True;";
        var original = Environment.GetEnvironmentVariable("ConnectionStrings__SqlServer");

        try
        {
            Environment.SetEnvironmentVariable("ConnectionStrings__SqlServer", null);
            ConnectionStringProvider.ResetCache();

            var result = ConnectionStringProvider.GetConnectionString(fallback);

            Assert.Equal(fallback, result);
        }
        finally
        {
            ConnectionStringProvider.ResetCache();
            Environment.SetEnvironmentVariable("ConnectionStrings__SqlServer", original);
        }
    }

    [Fact]
    public void ReturnsEnvironmentValueWhenPresent()
    {
        const string envValue = "Server=envserver;Database=Db;User Id=sa;Password=P@ss;";
        var original = Environment.GetEnvironmentVariable("ConnectionStrings__SqlServer");

        try
        {
            Environment.SetEnvironmentVariable("ConnectionStrings__SqlServer", envValue);
            ConnectionStringProvider.ResetCache();

            var result = ConnectionStringProvider.GetConnectionString("ignored-fallback");

            Assert.Equal(envValue, result);
        }
        finally
        {
            ConnectionStringProvider.ResetCache();
            Environment.SetEnvironmentVariable("ConnectionStrings__SqlServer", original);
        }
    }
}
