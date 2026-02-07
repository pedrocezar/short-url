using FluentValidation;
using ShortUrl.Models;
using ShortUrl.Services;
using ShortUrl.Validators;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ShortUrlService>();
builder.Services.AddValidatorsFromAssemblyContaining<ShortenRequestValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/shorten", async (ShortenRequest req, ShortUrlService svc, IValidator<ShortenRequest> validator, HttpRequest http) =>
{
    var key = svc.Shorten(req.Url);
    var baseUrl = $"{http.Scheme}://{http.Host}";
    var shortUrl = $"{baseUrl}/{key}";
    return Results.Ok(new { key, shortUrl });
});

app.MapGet("/{key}", (string key, ShortUrlService svc) =>
{
    var url = svc.Resolve(key);
    if (url is null) return Results.NotFound();
    return Results.Redirect(url);
});

app.MapGet("/url/{key}", (string key, ShortUrlService svc) =>
{
    var url = svc.Resolve(key);
    if (url is null) return Results.NotFound();
    return Results.Ok(new { url });
});

await app.RunAsync();
