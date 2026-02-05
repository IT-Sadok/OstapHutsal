using CRMSystem.Application.Features.Auth.Contracts;
using FluentValidation;

namespace CRMSystem.Application.Features.Auth.Validators;

public class RegisterAgentRequestValidator : AbstractValidator<RegisterAgentRequest>
{
    public RegisterAgentRequestValidator()
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
    }
}