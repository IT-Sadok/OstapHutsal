using Common;
using CRMSystem.Application.Abstractions.Persistence;
using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Application.Abstractions.Services;
using CRMSystem.Application.Features.Tickets.Contracts;
using CRMSystem.Domain.DomainEvents.Tickets;
using CRMSystem.Domain.Entities.Factories;

namespace CRMSystem.Application.Features.Tickets;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IActorRepository _actorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TicketService(
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        IActorRepository actorRepository)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
        _actorRepository = actorRepository;
    }

    public async Task<Result<Guid>> CreateAsync(
        Guid createdByActorId,
        CreateTicketRequest request,
        Guid? createdToClientId = null,
        CancellationToken cancellationToken = default)
    {
        Guid clientId;

        if (createdToClientId is null)
        {
            var actor = await _actorRepository
                .GetByIdWithClientAsync(createdByActorId, cancellationToken);

            if (actor is null)
            {
                return Result<Guid>.Failure(TicketErrorCodes.ActorNotFound);
            }

            if (actor.Client is null)
            {
                return Result<Guid>.Failure(TicketErrorCodes.ClientNotFoundForActor);
            }

            clientId = actor.Client.Id;
        }
        else
        {
            clientId = createdToClientId.Value;
        }

        var ticket = TicketFactory.Create(
            clientId: clientId,
            categoryId: request.CategoryId,
            priorityId: request.PriorityId,
            channelId: request.ChannelId,
            title: request.Title,
            description: request.Description,
            sourceDetails: request.SourceDetails,
            orderId: request.OrderId
        );

        await _ticketRepository.AddAsync(ticket, cancellationToken);

        ticket.Raise(new TicketCreatedEvent(ticket.Id, createdByActorId, clientId));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(ticket.Id);
    }

    public async Task<Result> SetAssigneeAsync(
        Guid ticketId,
        Guid performedByActorId,
        bool isAdmin,
        AssignTicketRequest request,
        CancellationToken cancellationToken = default)
    {
        var performedBy = await _actorRepository.GetByIdAsync(performedByActorId, cancellationToken);
        if (performedBy is null)
            return Result.Failure(TicketErrorCodes.ActorNotFound);

        var ticket = await _ticketRepository.GetByIdAsync(ticketId, cancellationToken);
        if (ticket is null)
            return Result.Failure(TicketErrorCodes.TicketNotFound);

        if (request.AssignToActorId is null)
        {
            if (ticket.AssignedToActorId is null)
                return Result.Success();

            var oldAssignee = ticket.AssignedToActorId;
            ticket.AssignedToActorId = null;

            ticket.Raise(new TicketUnassignedEvent(
                ticket.Id,
                performedByActorId,
                oldAssignee
            ));

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        var newAssigneeActorId = request.AssignToActorId.Value;

        var assignee = await _actorRepository.GetByIdAsync(newAssigneeActorId, cancellationToken);
        if (assignee is null)
            return Result.Failure(TicketErrorCodes.ActorNotFound);

        if (!isAdmin && newAssigneeActorId != performedByActorId)
            return Result.Failure(TicketErrorCodes.ForbiddenAssignToOther);

        if (ticket.AssignedToActorId == newAssigneeActorId)
            return Result.Success();

        var oldAssigneeActorId = ticket.AssignedToActorId;
        ticket.AssignedToActorId = newAssigneeActorId;

        ticket.Raise(new TicketAssignedEvent(
            ticket.Id,
            newAssigneeActorId,
            performedByActorId,
            oldAssigneeActorId
        ));

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}