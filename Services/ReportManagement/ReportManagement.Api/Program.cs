using Microsoft.AspNetCore.Mvc;
using ReportManagement.Application;
using ReportManagement.Infrastructure.Data;
using Shared.Api.Common;
using Shared.Application.Messaging;
using Shared.Infrastructure.Messaging;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false; // This is to ensure that the async suffix is not added to action names
    options.Filters.Add<ValidationFilter>(); // Add the validation filter to handle model validation
})
.AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
);
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);


builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureData(builder.Configuration, builder.Environment.IsDevelopment());

builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, typeof(Program).Assembly);
builder.Services.AddScoped<IEventPublisher, EventPublisher>();

builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

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

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.Services.InitializeDatabase();

app.Run();
