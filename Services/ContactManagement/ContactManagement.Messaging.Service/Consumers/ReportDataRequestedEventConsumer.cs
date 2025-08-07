using ContactManagement.Application.EventHandlers;
using MassTransit;
using Shared.Contracts;
using System.Text.Json;

namespace ContactManagement.Messaging.Service.Consumers;

public class ReportDataRequestedEventConsumer : IConsumer<ReportDataRequestedEvent>
{
    private readonly ILogger<ReportDataRequestedEventConsumer> logger;
    private readonly IBus _bus;
    private readonly IReportDataRequestHandler reportDataRequestHandler;

    public ReportDataRequestedEventConsumer(ILogger<ReportDataRequestedEventConsumer> logger, IBus bus, IReportDataRequestHandler reportDataRequestHandler)
    {
        this.logger = logger;
        this._bus = bus;
        this.reportDataRequestHandler = reportDataRequestHandler;
    }
    public async Task Consume(ConsumeContext<ReportDataRequestedEvent> context)
    {
        logger.LogInformation("ReportDataRequestedEventConsumer: {Message}", JsonSerializer.Serialize(context.Message));
        logger.LogInformation("Creating ReporData");
        
        var reportDataCreatedEvent = await reportDataRequestHandler.GenerateReportData(context.Message);

        await _bus.Publish(reportDataCreatedEvent);
    }
}
