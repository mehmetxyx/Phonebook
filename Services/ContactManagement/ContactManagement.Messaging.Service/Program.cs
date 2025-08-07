using ContactManagement.Application.EventHandlers;
using Shared.Infrastructure.Messaging;
using ContactManagement.Infrastructure.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, typeof(Program).Assembly);
builder.Services.AddInfrastructureData(builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.AddScoped<IReportDataRequestEventHandler, ReportDataRequestEventHandler>();

var host = builder.Build();
host.Run();
