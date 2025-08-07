using ContactManagement.Application.EventHandlers;
using Shared.Infrastructure.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, typeof(Program).Assembly);
builder.Services.AddSingleton<IReportDataRequestHandler, ReportDataRequestHandler>();

var host = builder.Build();
host.Run();
