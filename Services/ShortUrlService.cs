using System.Collections.Concurrent;
using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;

namespace ShortUrl.Services;

public class ShortUrlService
{
    private readonly ConcurrentDictionary<string, string> _store = new();

    public string Shorten(string url)
    {
        if (_store.TryGetValue(url, out var existing)) return existing;

        var key = GenerateKey(6);
        if (_store.TryAdd(key, url)) return key;

        throw new InvalidOperationException("Failed to generate unique key for URL");
    }

    public string? Resolve(string key)
    {
        return _store.TryGetValue(key, out var url) ? url : null;
    }

    public static string GenerateKey(int size)
    {
        var bytes = RandomNumberGenerator.GetBytes(size);
        return WebEncoders.Base64UrlEncode(bytes);
    }
}
