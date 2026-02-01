using Common;
using CRMSystem.Application.Features.Tickets.Contracts;

namespace CRMSystem.Application.Abstractions.Services;

public interface ITicketService
{
    Task<Result<Guid>> CreateAsync(
        Guid createdByActorId,
        CreateTicketRequest request,
        Guid? createdToClientId = null,
        CancellationToken cancellationToken = default);

    Task<Result> SetAssigneeAsync(
        Guid ticketId,
        Guid performedByActorId,
        bool isAdmin,
        AssignTicketRequest request,
        CancellationToken cancellationToken = default);
}