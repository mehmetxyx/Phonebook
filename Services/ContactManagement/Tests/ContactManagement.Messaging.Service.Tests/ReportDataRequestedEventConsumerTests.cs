using AutoFixture;
using ContactManagement.Application.EventHandlers;
using ContactManagement.Messaging.Service.Consumers;
using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shared.Common;
using Shared.Contracts;

namespace ContactManagement.Messaging.Service.Tests;
public class ReportDataRequestedEventConsumerTests
{
    private readonly IFixture fixture;
    private readonly ILogger<ReportDataRequestedEventConsumer> logger;
    private readonly IBus bus;
    private readonly IReportDataRequestEventHandler handler;

    public ReportDataRequestedEventConsumerTests()
    {
        fixture = new Fixture();
        logger = Substitute.For<ILogger<ReportDataRequestedEventConsumer>>();
        bus = Substitute.For<IBus>();
        handler = Substitute.For<IReportDataRequestEventHandler>();
    }
    [Fact]
    public async Task Consume_WhenSuccessful_Generate_Report_And_Publish_Event()
    { 
        var reportDataRequestedEvent = fixture.Create<ReportDataRequestedEvent>();
        var reportDataCreatedEvent = fixture.Create<ReportDataCreatedEvent>();
        var reportDataCreatedEventResponse = Result<ReportDataCreatedEvent>.Success(reportDataCreatedEvent);

        var consumer = new ReportDataRequestedEventConsumer(logger, bus, handler);

        handler.GenerateReportDataAsync(reportDataRequestedEvent)
            .Returns(Task.FromResult(reportDataCreatedEventResponse));

        var context = Substitute.For<ConsumeContext<ReportDataRequestedEvent>>();
        context.Message.Returns(reportDataRequestedEvent);

        await consumer.Consume(context);
        await handler.Received(1).GenerateReportDataAsync(reportDataRequestedEvent);
        await bus.Received(1).Publish(reportDataCreatedEvent);
    }

    [Fact]
    public async Task Consume_WhenFailedToGenerateReport_NoMessagePublished()
    {
        var reportDataRequestedEvent = fixture.Create<ReportDataRequestedEvent>();
        var reportDataCreatedEventResponse = Result<ReportDataCreatedEvent>.Failure("Cant generate report data");

        var consumer = new ReportDataRequestedEventConsumer(logger, bus, handler);

        handler.GenerateReportDataAsync(reportDataRequestedEvent)
            .Returns(Task.FromResult(reportDataCreatedEventResponse));

        var context = Substitute.For<ConsumeContext<ReportDataRequestedEvent>>();
        context.Message.Returns(reportDataRequestedEvent);

        await consumer.Consume(context);
        await handler.Received(1).GenerateReportDataAsync(reportDataRequestedEvent);
        await bus.DidNotReceive().Publish(Arg.Any<ReportDataCreatedEvent>());
    }
}
