using AutoFixture;
using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ReportManagement.Application.EventHandlers;
using ReportManagement.Messaging.Service.Consumers;
using Shared.Common;
using Shared.Contracts;

namespace ReportManagement.Messaging.Service.Tests;

public class ReportDataCreatedEventConsumerTests
{
    private readonly IFixture fixture;
    private readonly ILogger<ReportDataCreatedEventConsumer> logger;
    private readonly IBus bus;
    private readonly IReportDataCreatedEventHandler handler;

    public ReportDataCreatedEventConsumerTests()
    {
        fixture = new Fixture();
        logger = Substitute.For<ILogger<ReportDataCreatedEventConsumer>>();
        bus = Substitute.For<IBus>();
        handler = Substitute.For<IReportDataCreatedEventHandler>();
    }

    [Fact]
    public async Task Consume_WhenSuccessful_SaveReportDataToDatabase()
    {
        var reportDataCreatedEvent = fixture.Create<ReportDataCreatedEvent>();
        var reportResponse = Result<bool>.Success(true);

        var consumer = new ReportDataCreatedEventConsumer(logger, bus, handler);

        handler.SaveReportDataAsync(reportDataCreatedEvent)
            .Returns(Task.FromResult(reportResponse));

        var context = Substitute.For<ConsumeContext<ReportDataCreatedEvent>>();
        context.Message.Returns(reportDataCreatedEvent);

        await consumer.Consume(context);

        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
    }

    [Fact]
    public async Task Consume_WhenFailed_WillNotSaveReportDataToDatabase()
    {
        var reportDataCreatedEvent = fixture.Create<ReportDataCreatedEvent>();
        var reportResponse = Result<bool>.Failure(false);

        var consumer = new ReportDataCreatedEventConsumer(logger, bus, handler);

        handler.SaveReportDataAsync(reportDataCreatedEvent)
            .Returns(Task.FromResult(reportResponse));

        var context = Substitute.For<ConsumeContext<ReportDataCreatedEvent>>();
        context.Message.Returns(reportDataCreatedEvent);

        await consumer.Consume(context);

        logger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
        }
}
