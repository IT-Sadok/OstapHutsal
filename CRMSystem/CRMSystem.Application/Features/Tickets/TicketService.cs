using Common;
using CRMSystem.Application.Abstractions.DomainEvents;
using CRMSystem.Application.Abstractions.Persistence;
using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Application.Abstractions.Services;
using CRMSystem.Application.Common.Authorization;
using CRMSystem.Application.Common.Errors;
using CRMSystem.Application.Common.Security;
using CRMSystem.Application.Features.Auth;
using CRMSystem.Application.Features.Tickets.Contracts;
using CRMSystem.Application.Features.Tickets.Snapshots;
using CRMSystem.Application.Identity;
using CRMSystem.Domain.DomainEvents.Tickets;
using CRMSystem.Domain.Entities.Factories;

namespace CRMSystem.Application.Features.Tickets;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IActorRepository _actorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextProvider _userContextProvider;
    private readonly IDomainEventsDispatcher _domainEventsDispatcher;
    private readonly ITicketSnapshotRepository _ticketSnapshotRepository;


    public TicketService(
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        IActorRepository actorRepository,
        IUserContextProvider userContextProvider,
        IDomainEventsDispatcher domainEventsDispatcher,
        ITicketSnapshotRepository ticketSnapshotRepository)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
        _actorRepository = actorRepository;
        _userContextProvider = userContextProvider;
        _domainEventsDispatcher = domainEventsDispatcher;
        _ticketSnapshotRepository = ticketSnapshotRepository;
    }

    public async Task<Result<Guid>> CreateAsync(
        CreateTicketRequest request,
        Guid? createdToClientId = null,
        CancellationToken cancellationToken = default)
    {
        Guid clientId;

        var userContext = _userContextProvider.FromHttpContext();

        if (userContext is null)
        {
            return Result<Guid>.Failure(CommonErrorCodes.Unauthorized);
        }

        if (createdToClientId is null)
        {
            var actor = await _actorRepository
                .GetByIdWithClientAsync(userContext.ActorId, cancellationToken);

            if (actor is null)
            {
                return Result<Guid>.Failure(CommonErrorCodes.NotFound);
            }

            if (actor.Client is null)
            {
                return Result<Guid>.Failure(CommonErrorCodes.NotFound);
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

        var ticketCreated = new TicketCreatedEvent(ticket.Id, userContext.ActorId, clientId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _domainEventsDispatcher.DispatchAsync([ticketCreated], cancellationToken);

        return Result<Guid>.Success(ticket.Id);
    }

    public async Task<Result> SetAssigneeAsync(
        Guid ticketId,
        AssignTicketRequest request,
        CancellationToken cancellationToken = default)
    {
        var userContext = _userContextProvider.FromHttpContext();

        if (userContext is null)
        {
            return Result.Failure(CommonErrorCodes.Unauthorized);
        }

        var performedBy = await _actorRepository.GetByIdAsync(userContext.ActorId, cancellationToken);
        if (performedBy is null)
            return Result.Failure(CommonErrorCodes.NotFound);

        var ticket = await _ticketRepository.GetByIdAsync(ticketId, cancellationToken);
        if (ticket is null)
            return Result.Failure(CommonErrorCodes.NotFound);

        var snapshot = TicketSnapshotFactory.Create(ticket, userContext.ActorId);

        if (request.AssignToActorId is null)
        {
            if (ticket.AssignedToActorId is null)
                return Result.Success();

            var oldAssignee = ticket.AssignedToActorId;
            ticket.AssignedToActorId = null;

            var ticketUnassigned = new TicketUnassignedEvent(
                ticket.Id,
                userContext.ActorId,
                oldAssignee
            );

            ticket.Version++;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _domainEventsDispatcher.DispatchAsync([ticketUnassigned], cancellationToken);

            return Result.Success();
        }

        var newAssigneeActorId = request.AssignToActorId.Value;

        var assignee = await _actorRepository.GetByIdAsync(newAssigneeActorId, cancellationToken);
        if (assignee is null)
            return Result.Failure(CommonErrorCodes.NotFound);

        if (!userContext.Roles.Contains(Roles.Admin) && newAssigneeActorId != userContext.ActorId)
            return Result.Failure(TicketErrorCodes.ForbiddenAssignToOther);

        if (ticket.AssignedToActorId == newAssigneeActorId)
            return Result.Success();

        var oldAssigneeActorId = ticket.AssignedToActorId;
        ticket.AssignedToActorId = newAssigneeActorId;

        var ticketAssigned = new TicketAssignedEvent(
            ticket.Id,
            newAssigneeActorId,
            userContext.ActorId,
            oldAssigneeActorId
        );

        await _ticketSnapshotRepository.AddAsync(snapshot, cancellationToken);
        ticket.Version++;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _domainEventsDispatcher.DispatchAsync([ticketAssigned], cancellationToken);

        return Result.Success();
    }

// WARNING: race condition on update
}