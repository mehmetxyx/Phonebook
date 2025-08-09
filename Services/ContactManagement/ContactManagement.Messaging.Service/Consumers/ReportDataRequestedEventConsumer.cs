using ContactManagement.Application.EventHandlers;
using MassTransit;
using Shared.Contracts;
using System.Text.Json;

namespace ContactManagement.Messaging.Service.Consumers;

public class ReportDataRequestedEventConsumer : IConsumer<ReportDataRequestedEvent>
{
    private readonly ILogger<ReportDataRequestedEventConsumer> logger;
    private readonly IBus _bus;
    private readonly IReportDataRequestEventHandler reportDataRequestHandler;

    public ReportDataRequestedEventConsumer(ILogger<ReportDataRequestedEventConsumer> logger, IBus bus, IReportDataRequestEventHandler reportDataRequestHandler)
    {
        this.logger = logger;
        this._bus = bus;
        this.reportDataRequestHandler = reportDataRequestHandler;
    }
    public async Task Consume(ConsumeContext<ReportDataRequestedEvent> context)
    {
        //TODO: remove
        logger.LogInformation("ReportDataRequestedEventConsumer: {Message}", JsonSerializer.Serialize(context.Message));
        logger.LogInformation("Creating ReporData for {ReportId}", context.Message.ReportId);
        
        var reportDataCreatedEvent = await reportDataRequestHandler.GenerateReportDataAsync(context.Message);

        if(!reportDataCreatedEvent.IsSuccess || reportDataCreatedEvent.Value is null)
        {
            logger.LogError("Failed to create report data for ReportId: {ReportId}. Error: {Error}", context.Message.ReportId, reportDataCreatedEvent.Message);
            //TODO: Handle failure appropriately, e.g., retry logic, dead-letter queue, etc.
            return;
        }
        
        await _bus.Publish(reportDataCreatedEvent.Value);
    }
}
