using LigaLibre.API.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace LigaLibre.Tests.Middlewares;

public class RequestLoggingMiddlewareTests
{
    private readonly Mock<ILogger<RequestLoggingMiddleware>> _mockLogger;
    private readonly Mock<RequestDelegate> _mockNext;

    public RequestLoggingMiddlewareTests()
    {
        _mockLogger = new Mock<ILogger<RequestLoggingMiddleware>>();
        _mockNext = new Mock<RequestDelegate>();
    }

    [Fact]
    public async Task InvokeAsync_LogsRequestAndResponse()
    {
        var middleware = new RequestLoggingMiddleware(_mockNext.Object, _mockLogger.Object);
        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.Path = "/api/test";

        await middleware.InvokeAsync(context);

        _mockLogger.Verify(l => l.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Iniciando")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);

        _mockLogger.Verify(l => l.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Completado")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_ErrorStatus_LogsWarning()
    {
        var middleware = new RequestLoggingMiddleware(_mockNext.Object, _mockLogger.Object);
        var context = new DefaultHttpContext();
        context.Response.StatusCode = 500;

        await middleware.InvokeAsync(context);

        _mockLogger.Verify(l => l.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Completado")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}
