using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Distributed;

namespace ShortUrl.Services;

public class ShortUrlService(IDistributedCache _cache)
{
    private static readonly string _keyPrefix = "key:url:";

    public async Task<string> ShortenAsync(string url, CancellationToken ct = default)
    {
        var key = GenerateKey(6);
        await _cache.SetStringAsync($"{_keyPrefix}{key}", url, new DistributedCacheEntryOptions { AbsoluteExpiration = DateTime.UtcNow.AddDays(1) }, ct);
        return key;
    }

    public async Task<string?> ResolveAsync(string key, CancellationToken ct = default)
    {
        return await _cache.GetStringAsync($"{_keyPrefix}{key}", ct);
    }

    private static string GenerateKey(int size)
    {
        var bytes = RandomNumberGenerator.GetBytes(size);
        return WebEncoders.Base64UrlEncode(bytes);
    }
}
