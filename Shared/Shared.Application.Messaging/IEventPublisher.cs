namespace Shared.Application.Messaging;
public interface IEventPublisher
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default);
}