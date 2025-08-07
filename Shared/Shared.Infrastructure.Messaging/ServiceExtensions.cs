using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Infrastructure.Messaging;

public static class ServiceExtensions
{
    public static void AddMassTransitWithRabbitMq(this IServiceCollection serviceCollection, IConfigurationManager configurationManager, Assembly consumerAssembly)
    {
        serviceCollection.Configure<RabbitMqSettings>(configurationManager.GetSection(nameof(RabbitMqSettings)));

        var settings = configurationManager.GetSection(nameof(RabbitMqSettings))
            .Get<RabbitMqSettings>();

        serviceCollection.AddMassTransit(busRegistrationConfigurator =>
        {
            busRegistrationConfigurator.SetKebabCaseEndpointNameFormatter();

            busRegistrationConfigurator.AddConsumers(consumerAssembly);

            busRegistrationConfigurator.UsingRabbitMq((busRegistrationContext, busFactoryConfigurator) =>
            {
                busFactoryConfigurator.ConfigureEndpoints(busRegistrationContext);

                busFactoryConfigurator.Host($"amqp://{settings.Host}:{settings.Port}", hostConfigurator =>
                {
                    hostConfigurator.Username(settings.Username);
                    hostConfigurator.Password(settings.Password);
                });
            });
        });
    }
}