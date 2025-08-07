using MassTransit;
using ReportManagement.Application.EventHandlers;
using Shared.Contracts;
using System.Text.Json;

namespace ReportManagement.Messaging.Service.Consumers;
public class ReportDataCreatedEventConsumer : IConsumer<ReportDataCreatedEvent>
{
    private readonly ILogger<ReportDataCreatedEventConsumer> logger;
    private readonly IReportDataCreatedEventHandler reportDataCreatedEventHandler;

    public ReportDataCreatedEventConsumer(ILogger<ReportDataCreatedEventConsumer> logger, IBus bus, IReportDataCreatedEventHandler reportDataCreatedEventHandler)
    {
        this.logger = logger;
        this.reportDataCreatedEventHandler = reportDataCreatedEventHandler;
    }
    public async Task Consume(ConsumeContext<ReportDataCreatedEvent> context)
    {
        var result = await reportDataCreatedEventHandler.SaveReportDataAsync(context.Message);

        if (!result.IsSuccess)
        {
            logger.LogError("Failed to save report data: {Error}", result.Message);
            return;
        }

        logger.LogInformation("Report data saved successfully: ReportId {ReportId}", context.Message.ReportId);
    }
}
