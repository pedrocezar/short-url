using FluentValidation;
using FluentValidation.AspNetCore;
using ShortUrl.Models;
using ShortUrl.Services;
using ShortUrl.Validators;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddStackExchangeRedisCache(options =>
{
    var redis = builder.Configuration.GetConnectionString("Redis");
    if (string.IsNullOrWhiteSpace(redis)) throw new InvalidOperationException("Redis connection string is not set in appsettings.json");
    options.Configuration = redis;
});
builder.Services.AddSingleton<ShortUrlService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ShortenRequestValidator>();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGet("/", () =>
{
    return Results.Redirect("/scalar");
}).ExcludeFromDescription();

app.MapPost("/shorten", async (ShortenRequest req, ShortUrlService svc, IValidator<ShortenRequest> validator, HttpRequest http, CancellationToken ct) =>
{
    var validationResult = await validator.ValidateAsync(req, ct);
    if (!validationResult.IsValid) return Results.BadRequest(new { validationResult.Errors });
    var key = await svc.ShortenAsync(req.Url, ct);
    var shortUrl = $"{http.Scheme}://{http.Host}/{key}";
    return Results.Ok(new { key, shortUrl });
});

app.MapGet("/{key}", async (string key, ShortUrlService svc, CancellationToken ct) =>
{
    var url = await svc.ResolveAsync(key, ct);
    if (url is null) return Results.NotFound();
    return Results.Redirect(url);
}).ExcludeFromDescription();

app.MapGet("/url/{key}", async (string key, ShortUrlService svc, CancellationToken ct) =>
{
    var url = await svc.ResolveAsync(key, ct);
    if (url is null) return Results.NotFound();
    return Results.Ok(new { url });
});

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.OpenApiRoutePattern = "/openapi/{documentName}.json";
});

await app.RunAsync();
