using System.Collections.Concurrent;

namespace LigaLibre.API.Middlewares;

public class RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
{
    //esto hace un rate limiting simple por IP
    private static readonly ConcurrentDictionary<string, List<DateTime>> _request = new();
    //indicamos la cantidad maxima de requests permitidos por IP en un lapso de tiempo
    private readonly int _maxRequests = 100;
    //capturamos tiempo de ventana
    private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1);

    public async Task InvokeAsync(HttpContext context)
    {
        //obtenemos la IP del cliente
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        //tomamos el tiempo actual
        var now = DateTime.UtcNow;
        _request.AddOrUpdate(clientIp, new List<DateTime> { now },
            (key, existing) =>
            {
                //limpiamos los requests viejos
                existing.RemoveAll(time => now - time > _timeWindow);
                existing.Add(now);
                return existing;
            });
        if (_request[clientIp].Count > _maxRequests)
        {
            logger.LogWarning("Rate limit excedido para IP: {IP}", clientIp);
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;

            await context.Response.WriteAsync("Demasiadas solicitudes.Por favor, intente de nuevo mas tarde.");
            return;
        }
        await next(context);
    }
}
