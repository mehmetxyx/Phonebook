using ContactManagement.Infrastructure.Data;
using ContactManagement.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false; // This is to ensure that the async suffix is not added to action names
});

builder.Services.AddOpenApi();
builder.Services.AddApplicationServices();
builder.Services.AddContactManagementInfrastructureData(builder.Configuration, builder.Environment.IsDevelopment());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
