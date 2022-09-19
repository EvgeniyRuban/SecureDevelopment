using FluentValidation;

namespace CardStorageService.Domain;

public sealed class ClientToCreateValidator : AbstractValidator<ClientToCreate>
{
    public ClientToCreateValidator()
    {
        RuleFor(p => p.Firstname)
            .NotNull()
            .Length(0, 255);

        RuleFor(p => p.Surname)
           .NotNull()
           .Length(0, 255);
    }
}