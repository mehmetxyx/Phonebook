using Microsoft.Extensions.DependencyInjection;
using ReportManagement.Application.Services;

namespace ReportManagement.Application;
public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IReportService, ReportService>();
        serviceCollection.AddScoped<IReportDataService, ReportDataService>();
    }
}
