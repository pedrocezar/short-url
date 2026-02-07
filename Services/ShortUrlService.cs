using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace ShortUrl.Services;

public class ShortUrlService
{
    private readonly ConcurrentDictionary<string, string> _store = new();
    private readonly ConcurrentDictionary<string, string> _reverse = new();
    private static readonly char[] Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
    private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

    public string Shorten(string url)
    {
        if (_reverse.TryGetValue(url, out var existing)) return existing;

        for (int i = 0; i < 10; i++)
        {
            var key = GenerateKey(6);
            if (_store.TryAdd(key, url))
            {
                _reverse.TryAdd(url, key);
                return key;
            }
        }

        throw new Exception("Failed to generate unique key for URL");
    }

    public string? Resolve(string key)
    {
        return _store.TryGetValue(key, out var url) ? url : null;
    }

    private string GenerateKey(int length)
    {
        var bytes = new byte[length];
        _rng.GetBytes(bytes);
        var sb = new StringBuilder(length);
        for (int i = 0; i < length; i++) sb.Append(Alphabet[bytes[i] % Alphabet.Length]);
        return sb.ToString();
    }
}
