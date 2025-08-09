using AutoFixture;
using MassTransit;
using Moq;
using NSubstitute;
using Shared.Contracts;

namespace Shared.Infrastructure.Messaging.Tests;

public class EventPublisherTests
{
    [Fact]
    public async Task PublishAsync_WhenCalled_Publishes_Event()
    {
        var publishEndpoint= Substitute.For<IPublishEndpoint>();
        var fixture = new Fixture();
        EventPublisher publisher = new EventPublisher(publishEndpoint);

        var eventRequest = fixture.Create<ReportDataRequestedEvent>();

        await publisher.PublishAsync(eventRequest);

        publishEndpoint.Received(1)
            .Publish(eventRequest, Arg.Any<CancellationToken>());
    }
}
