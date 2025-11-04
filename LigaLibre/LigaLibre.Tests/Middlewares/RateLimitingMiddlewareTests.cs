using LigaLibre.API.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace LigaLibre.Tests.Middlewares;

public class RateLimitingMiddlewareTests
{
    private readonly Mock<ILogger<RateLimitingMiddleware>> _mockLogger;
    private readonly Mock<RequestDelegate> _mockNext;

    public RateLimitingMiddlewareTests()
    {
        _mockLogger = new Mock<ILogger<RateLimitingMiddleware>>();
        _mockNext = new Mock<RequestDelegate>();
    }

    [Fact]
    public async Task InvokeAsync_SetsStatusCode429_WhenOverLimit()
    {
        var middleware = new RateLimitingMiddleware(_mockNext.Object, _mockLogger.Object);
        var context = new DefaultHttpContext();
        context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.1.100");
        context.Response.Body = new MemoryStream();

        for (int i = 0; i < 101; i++)
        {
            context = new DefaultHttpContext();
            context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.1.100");
            context.Response.Body = new MemoryStream();
            await middleware.InvokeAsync(context);
        }

        Assert.Equal(StatusCodes.Status429TooManyRequests, context.Response.StatusCode);
    }
}
