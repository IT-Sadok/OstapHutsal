using CRMSystem.Application.Features.Auth.Contracts;
using FluentValidation;

namespace CRMSystem.Application.Features.Auth.Validators;

public class RegisterClientRequestValidator : AbstractValidator<RegisterClientRequest>
{
    public RegisterClientRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Phone)
            .MaximumLength(32)
            .When(x => x.Phone is not null);

        RuleFor(x => x.Address)
            .MaximumLength(512)
            .When(x => x.Address is not null);
    }
}