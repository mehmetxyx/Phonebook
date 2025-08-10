using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text.Json;

namespace Shared.Api.Common.Tests;

public class GlobalExceptionHandlerMiddlewareTests
{
    [Fact]
    public async Task GlobalExceptionHandlerMiddleware_WhenUnhandledExceptionOccurs_Returns_CorrectApiResponse()
    {
        var logger = Substitute.For<ILogger<GlobalExceptionHandlerMiddleware>>();

        var middleware = new GlobalExceptionHandlerMiddleware(logger);

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var next = Substitute.For<RequestDelegate>();
        
        next.Invoke(context)
            .Returns(d => throw new Exception("Test exception"));

        await middleware.InvokeAsync(context, next);

        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Equal("application/json; charset=utf-8", context.Response.ContentType);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();

        var response = JsonSerializer.Deserialize<ApiResponse<string>>(responseBody);
        Assert.NotNull(response);
        Assert.False(response!.IsSuccessful);
    }
}
