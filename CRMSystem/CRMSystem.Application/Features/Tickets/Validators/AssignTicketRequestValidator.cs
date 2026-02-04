using CRMSystem.Application.Features.Tickets.Contracts;
using FluentValidation;

namespace CRMSystem.Application.Features.Tickets.Validators;

public class AssignTicketRequestValidator : AbstractValidator<AssignTicketRequest>
{
    public AssignTicketRequestValidator()
    {
        RuleFor(x => x.AssignToActorId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("OrderId must be a valid GUID")
            .When(x => x.AssignToActorId.HasValue);
    }
}