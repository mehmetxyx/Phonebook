using Shared.Infrastructure.Messaging;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, typeof(Program).Assembly);

var host = builder.Build();
host.Run();
