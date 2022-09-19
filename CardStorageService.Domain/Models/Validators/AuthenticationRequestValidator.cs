using FluentValidation;

namespace CardStorageService.Domain;

public sealed class AuthenticationRequestValidator : AbstractValidator<AuthenticationRequest>
{
    public AuthenticationRequestValidator()
    {
        RuleFor(p => p.Login)
            .NotEmpty()
            .NotNull()
            .Length(5, 100);

        RuleFor(p => p.Login)
            .NotEmpty()
            .NotNull()
            .Length(5, 100);
    }
}