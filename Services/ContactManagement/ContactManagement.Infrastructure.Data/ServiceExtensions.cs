using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ContactManagement.Infrastructure.Data;
public static class ServiceExtensions
{
    public static void AddContactManagementInfrastructureData(this IServiceCollection serviceCollection, IConfiguration configuration, bool logSensitiveAndDetailedLogs)
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
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
