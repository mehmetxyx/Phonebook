using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportManagement.Application.Interfaces;
using ReportManagement.Domain.Repositories;
using ReportManagement.Infrastructure.Data.Repositories;

namespace ReportManagement.Infrastructure.Data;

public static class ServiceExtensions
{
    public static void AddInfrastructureData(this IServiceCollection serviceCollection, IConfiguration configuration, bool logSensitiveAndDetailedLogs)
    {
        serviceCollection.AddDbContext<ReportManagementDbContext>(optionsBuilder =>
        {
            var connectionString = configuration.GetConnectionString("ReportManagementDb");
            optionsBuilder.UseNpgsql(connectionString, npgSqlOptions =>
            {
                npgSqlOptions.MigrationsHistoryTable("ef_migrations_history", "public");
            });

            optionsBuilder.UseSnakeCaseNamingConvention();

            optionsBuilder.EnableSensitiveDataLogging(logSensitiveAndDetailedLogs);
            optionsBuilder.EnableDetailedErrors(logSensitiveAndDetailedLogs);
        });
        
        serviceCollection.AddScoped<IReportRepository, ReportRepository>();
        serviceCollection.AddScoped<IReportDataRepository, ReportDataRepository>();
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
