using LigaLibre.API.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace LigaLibre.Tests.Middlewares;

public class ErrorLoggingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_NoException_CallsNext()
    {
        //Arrange
        var context = new DefaultHttpContext();

        var nextCalled = false;

        RequestDelegate next = (HttpContext ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        var mockLogger = new Moq.Mock<ILogger<ErrorLoggingMiddleware>>();
        var middleware = new ErrorLoggingMiddleware(next, mockLogger.Object);
        //Act

        await middleware.InvokeAsync(context);
        //Assert
        Assert.True(nextCalled);
    }

    [Fact]
    public async Task InvokeAsync_ExceptionThrown_LogsError()
    {
        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.Path = "/api/test";

        RequestDelegate next = (HttpContext ctx) => throw new Exception("Test exception");

        var mockLogger = new Mock<ILogger<ErrorLoggingMiddleware>>();
        var middleware = new ErrorLoggingMiddleware(next, mockLogger.Object);

        await middleware.InvokeAsync(context);

        mockLogger.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}
