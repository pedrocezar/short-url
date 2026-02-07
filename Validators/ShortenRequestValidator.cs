using FluentValidation;
using ShortUrl.Models;

namespace ShortUrl.Validators;

public class ShortenRequestValidator : AbstractValidator<ShortenRequest>
{
    public ShortenRequestValidator()
    {
        RuleFor(x => x.Url)
            .NotNull()
            .NotEmpty()
            .WithMessage("URL is required")
            .Must(IsValidUrl)
            .WithMessage("URL must be a valid URL");
    }

    private static bool IsValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
