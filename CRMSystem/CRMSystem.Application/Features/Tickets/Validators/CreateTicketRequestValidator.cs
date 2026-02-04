using CRMSystem.Application.Features.Tickets.Contracts;
using FluentValidation;

namespace CRMSystem.Application.Features.Tickets.Validators;

public class CreateTicketRequestValidator : AbstractValidator<CreateTicketRequest>
{
    public CreateTicketRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .When(x => x.Description is not null);

        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.PriorityId)
            .NotEmpty();

        RuleFor(x => x.ChannelId)
            .NotEmpty();

        RuleFor(x => x.OrderId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("OrderId must be a valid GUID")
            .When(x => x.OrderId.HasValue);

        RuleFor(x => x.SourceDetails)
            .MaximumLength(2000)
            .When(x => x.SourceDetails is not null);
    }
}