var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

app.MapFallbackToFile("index.html");


app.MapGet("/config.js", (IConfiguration config) =>
{
    var contactApiBaseUrl = config.GetSection("ApiConfigs").GetValue<string>("ContactApiBaseUrl");
    var reportApiBaseUrl = config.GetSection("ApiConfigs").GetValue<string>("ReportApiBaseUrl");

    return Results.Text($@"
        window.contactsApi = '{contactApiBaseUrl}';
        window.reportsApi = '{reportApiBaseUrl}';
    ", "application/javascript");
});


app.Run();
