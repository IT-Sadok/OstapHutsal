using Common;
using CRMSystem.Application.Common.Security;
using CRMSystem.Application.Features.Tickets.Contracts;

namespace CRMSystem.Application.Abstractions.Services;

public interface ITicketService
{
    Task<Result<Guid>> CreateAsync(
        CreateTicketRequest request,
        Guid? createdToClientId = null,
        CancellationToken cancellationToken = default);

    Task<Result> SetAssigneeAsync(
        Guid ticketId,
        AssignTicketRequest request,
        CancellationToken cancellationToken = default);
}