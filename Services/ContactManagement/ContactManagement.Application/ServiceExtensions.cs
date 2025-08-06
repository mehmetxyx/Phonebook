using ContactManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ContactManagement.Application;
public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IContactService, ContactService>();
        serviceCollection.AddScoped<IContactDetailService, ContactDetailService>();
    }
}
