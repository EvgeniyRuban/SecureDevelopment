using FluentValidation;

namespace CardStorageService.Domain;

public sealed class CardToCreateValidator : AbstractValidator<CardToCreate>
{
    public CardToCreateValidator()
    {
        RuleFor(p => p.Number)
            .NotNull()
            .Length(16, 16);

        RuleFor(p => p.CVV2)
            .NotNull()
            .Length(3, 3);
    }
}