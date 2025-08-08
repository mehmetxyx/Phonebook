using ReportManagement.Application;
using ReportManagement.Infrastructure.Data;
using Shared.Application.Messaging;
using Shared.Infrastructure.Messaging;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false; // This is to ensure that the async suffix is not added to action names
})
.AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureData(builder.Configuration, builder.Environment.IsDevelopment());

builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, typeof(Program).Assembly);
builder.Services.AddScoped<IEventPublisher, EventPublisher>();
builder.Services.AddOpenApi();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

//todo: Add CORS policy to allow requests from the Phonebook client
app.UseCors(options =>
{
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});


app.Run();
