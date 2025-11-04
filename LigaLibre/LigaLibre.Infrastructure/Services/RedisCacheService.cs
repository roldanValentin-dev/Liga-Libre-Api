using LigaLibre.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace LigaLibre.Infrastructure.Services;

public class RedisCacheService(IDistributedCache cache) : IRedisCacheService
{


    //Buscar en Redis por Key
    public async Task<T?> GetAsync<T>(string key)
    {

        var value = await cache.GetStringAsync(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }

    //Guarda un objeto con TTL (tiempo de vida)
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions();

        if (expiration.HasValue)
            options.SetAbsoluteExpiration(expiration.Value);
        else
            options.SetSlidingExpiration(TimeSpan.FromMinutes(30));

        await cache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
    }
    //Remueve el cache a traves de la key
    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }

    //Remueve siguiendo un patron
    public async Task RemovePatternAsync(string pattern)
    {
        if (pattern == "clubs:*")
        {
            await RemoveAsync("clubs:all");
        }
    }
}
