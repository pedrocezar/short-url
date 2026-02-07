using FluentValidation;
using FluentValidation.AspNetCore;
using ShortUrl.Models;
using ShortUrl.Services;
using ShortUrl.Validators;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ShortUrlService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ShortenRequestValidator>();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGet("/", () =>
{
    return Results.Redirect("/scalar");
}).ExcludeFromDescription();

app.MapPost("/shorten", async (ShortenRequest req, ShortUrlService svc, IValidator<ShortenRequest> validator, HttpRequest http) =>
{
    var validationResult = await validator.ValidateAsync(req);
    if (!validationResult.IsValid) return Results.BadRequest(new { validationResult.Errors });
    var key = svc.Shorten(req.Url);
    var shortUrl = $"{http.Scheme}://{http.Host}/{key}";
    return Results.Ok(new { key, shortUrl });
});

app.MapGet("/{key}", (string key, ShortUrlService svc) =>
{
    var url = svc.Resolve(key);
    if (url is null) return Results.NotFound();
    return Results.Redirect(url);
}).ExcludeFromDescription();

app.MapGet("/url/{key}", (string key, ShortUrlService svc) =>
{
    var url = svc.Resolve(key);
    if (url is null) return Results.NotFound();
    return Results.Ok(new { url });
});

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.OpenApiRoutePattern = "/openapi/{documentName}.json";
});

await app.RunAsync();
