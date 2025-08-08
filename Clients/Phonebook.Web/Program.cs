var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

app.MapFallbackToFile("index.html");

app.MapGet("/config.js", () =>
{
    var contactsApi = Environment.GetEnvironmentVariable("CONTACT_MANAGEMENT_API") ?? "http://localhost:5074/api/contacts";
    var reportsApi = Environment.GetEnvironmentVariable("REPORTS_MANAGEMENT_API") ?? "http://localhost:5012/api/reports";

    return Results.Text($@"
        window.contactsApi = '{contactsApi}';
        window.reportsApi = '{reportsApi}';
    ", "application/javascript");
});


app.Run();
