using FluentValidation;
using ShortUrl.Models;

namespace ShortUrl.Validators;

public class ShortenRequestValidator : AbstractValidator<ShortenRequest>
{
    public ShortenRequestValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty()
            .WithMessage("Url é obrigatória")
            .Must(IsValidUrl)
            .WithMessage("Url deve ser uma URL válida");
    }

    private static bool IsValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
