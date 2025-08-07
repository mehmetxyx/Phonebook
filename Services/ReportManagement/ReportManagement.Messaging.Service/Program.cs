using ReportManagement.Application.EventHandlers;
using ReportManagement.Infrastructure.Data;
using Shared.Infrastructure.Messaging;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructureData(builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, typeof(Program).Assembly);
builder.Services.AddScoped<IReportDataCreatedEventHandler, ReportDataCreatedEventHandler>();
var host = builder.Build();
host.Run();
