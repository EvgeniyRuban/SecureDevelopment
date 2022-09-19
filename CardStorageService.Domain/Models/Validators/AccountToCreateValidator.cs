using FluentValidation;

namespace CardStorageService.Domain;

public sealed class AccountToCreateValidator : AbstractValidator<AccountToCreate>
{
    public AccountToCreateValidator()
    {
        RuleFor(p => p.Email)
            .NotNull()
            .NotEmpty()
            .Length(5, 255)
            .EmailAddress();

        RuleFor(p => p.Password)
            .NotNull()
            .NotEmpty()
            .Length(5, 100);

        RuleFor(p => p.Firstname)
            .NotNull()
            .NotEmpty()
            .Length(5, 100);
    }
}