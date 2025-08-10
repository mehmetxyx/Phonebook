using ContactManagement.Application;
using ContactManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Common;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false; // This is to ensure that the async suffix is not added to action names
    options.Filters.Add<ValidationFilter>();
})
.AddJsonOptions(options => 
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
);
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureData(builder.Configuration, builder.Environment.IsDevelopment());


builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Services.InitializeDatabase();

//todo: Add CORS policy to allow requests from the Phonebook client
app.UseCors(options => 
{
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
