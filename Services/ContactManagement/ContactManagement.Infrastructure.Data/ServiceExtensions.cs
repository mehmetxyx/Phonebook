using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ContactManagement.Application.Repositories;

namespace ContactManagement.Infrastructure.Data;
public static class ServiceExtensions
{
    public static void AddInfrastructureData(this IServiceCollection serviceCollection, IConfiguration configuration, bool logSensitiveAndDetailedLogs)
    {
        serviceCollection.AddDbContext<ContactManagementDbContext>(optionsBuilder =>
        {
            var connectionString = configuration.GetConnectionString("ContactManagementDb");
            optionsBuilder.UseNpgsql(connectionString, npgSqlOptions =>
            {
                npgSqlOptions.MigrationsHistoryTable("ef_migrations_history", "public");
            });

            optionsBuilder.UseSnakeCaseNamingConvention();

            optionsBuilder.EnableSensitiveDataLogging(logSensitiveAndDetailedLogs);
            optionsBuilder.EnableDetailedErrors(logSensitiveAndDetailedLogs);
        });
        
        serviceCollection.AddScoped<IContactRepository, ContactRepository>();
        serviceCollection.AddScoped<IContactDetailRepository, ContactDetailRepository>();
        serviceCollection.AddScoped<IContactReportDataRepository, ContactReportDataRepository>();
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
