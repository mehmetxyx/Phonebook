using MassTransit;
using Shared.Application.Messaging;

namespace Shared.Infrastructure.Messaging;
public class EventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventPublisher(IPublishEndpoint publishEndpoint)
    {
        this._publishEndpoint = publishEndpoint;
    }
    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : class
    {
        return _publishEndpoint.Publish(message, cancellationToken);
    }
}