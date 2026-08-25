using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace OrderManagement.IntegrationTests;

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseDirectory;
    private readonly string _databasePath;

    public ApiFactory()
    {
        _databaseDirectory = Path.Combine(Path.GetTempPath(), "OrderManagement.IntegrationTests", Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(_databaseDirectory);

        _databasePath = Path.Combine(_databaseDirectory, "orders.db");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
            {
                Dictionary<string, string?> testConfiguration = new()
                {
                    ["DatabasePath"] = _databasePath
                };

                configurationBuilder.AddInMemoryCollection(
                    testConfiguration);
            });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            base.Dispose(disposing);

            SqliteConnection.ClearAllPools();

            if (Directory.Exists(_databaseDirectory))
            {
                Directory.Delete(
                    _databaseDirectory,
                    recursive: true);
            }

            return;
        }

        base.Dispose(disposing);
    }
}