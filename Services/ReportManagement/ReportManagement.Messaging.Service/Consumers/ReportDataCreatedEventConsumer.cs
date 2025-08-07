using MassTransit;
using Shared.Contracts;
using System.Text.Json;

namespace ReportManagement.Messaging.Service.Consumers;
public class ReportDataCreatedEventConsumer : IConsumer<ReportDataCreatedEvent>
{
    private readonly ILogger<ReportDataCreatedEventConsumer> logger;

    public ReportDataCreatedEventConsumer(ILogger<ReportDataCreatedEventConsumer> logger)
    {
        this.logger = logger;
    }
    public Task Consume(ConsumeContext<ReportDataCreatedEvent> context)
    {
        logger.LogInformation("ReportDataCreatedEventConsumer: {Message}", JsonSerializer.Serialize(context.Message));
        return Task.CompletedTask;
    }
}
