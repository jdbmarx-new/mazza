using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Abstractions;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databasePath = Path.Combine(configuration["DatabasePath"] ?? "DB", configuration["DatabaseName"] ?? "orders.db");

        string absoluteDatabasePath = Path.IsPathRooted(databasePath)
                                            ? databasePath
                                            : Path.GetFullPath(databasePath);

        string? databaseDirectory = Path.GetDirectoryName(absoluteDatabasePath);

        if (!string.IsNullOrWhiteSpace(databaseDirectory))
        {
            Directory.CreateDirectory(databaseDirectory);
        }

        string connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = absoluteDatabasePath,
            Mode = SqliteOpenMode.ReadWriteCreate,
            Pooling = true
        }.ToString();

        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddSingleton<IClock, SystemClock>();

        return services;
    }
}