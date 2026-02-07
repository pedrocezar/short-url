using ShortUrl.Models;
using ShortUrl.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ShortUrlService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/shorten", (ShortenRequest req, ShortUrlService svc, HttpRequest http) =>
{
    if (req is null || string.IsNullOrWhiteSpace(req.Url))
        return Results.BadRequest(new { error = "Url is required" });

    var key = svc.Shorten(req.Url);
    var baseUrl = $"{http.Scheme}://{http.Host}";
    var shortUrl = $"{baseUrl}/{key}";
    return Results.Ok(new { key, shortUrl });
});

app.MapGet("/{key}", (string key, ShortUrlService svc) =>
{
    if (string.IsNullOrWhiteSpace(key))
        return Results.BadRequest(new { error = "Key is required" });

    var url = svc.Resolve(key);
    if (url is null) return Results.NotFound();
    return Results.Ok(new { url });
});

await app.RunAsync();
