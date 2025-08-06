using ReportManagement.Infrastructure.Data;
using ReportManagement.Application;
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

builder.Services.AddOpenApi();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
