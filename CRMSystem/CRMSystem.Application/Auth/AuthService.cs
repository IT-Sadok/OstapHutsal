using CRMSystem.Application.Abstractions.Identity;
using CRMSystem.Application.Abstractions.Persistence;
using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Application.Abstractions.Services;
using CRMSystem.Application.Auth.Contracts;
using CRMSystem.Application.Auth.Models;
using CRMSystem.Application.Common;
using CRMSystem.Application.Common.Authorization;
using CRMSystem.Domain.Entities.Factories;

namespace CRMSystem.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenProvider _jwtTokenProvider;
    private readonly IActorRepository _actorRepository;
    private readonly IAgentRepository _agentRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IIdentityService identityService,
        IJwtTokenProvider jwtTokenProvider,
        IActorRepository actorRepository,
        IAgentRepository agentRepository,
        IClientRepository clientRepository,
        IUnitOfWork unitOfWork)
    {
        _identityService = identityService;
        _jwtTokenProvider = jwtTokenProvider;
        _actorRepository = actorRepository;
        _agentRepository = agentRepository;
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> LoginAsync(LoginRequest loginRequest)
    {
        var result = await _identityService.AuthenticateAsync(loginRequest);

        if (!result.IsSuccess)
        {
            return Result<string>.Failure(result.ErrorCode);
        }

        var tokenResult = _jwtTokenProvider.Create(result.Value!);
        if (!tokenResult.IsSuccess)
        {
            return Result<string>.Failure(result.ErrorCode);
        }

        return Result<string>.Success(tokenResult.Value!);
    }

    public async Task<Result<Guid>> RegisterClientAsync(RegisterClientRequest request,
        CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var actor = ActorFactory.CreateClientActor(request.Phone, request.Address);
        await _actorRepository.AddAsync(actor);

        var userRegisterIdentity = new UserRegisterIdentity
        (
            FirstName: request.FirstName,
            LastName: request.LastName,
            Email: request.Email,
            Role: Roles.Client,
            ActorUser: actor,
            Password: request.Password
        );

        var userIdentityResult = await _identityService.CreateUserAsync(userRegisterIdentity);


        if (!userIdentityResult.IsSuccess)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result<Guid>.Failure(userIdentityResult.ErrorCode);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
        return Result<Guid>.Success(userIdentityResult.Value);
    }

    public async Task<Result<Guid>> RegisterOperatorAsync(RegisterAgentRequest request,
        CancellationToken cancellationToken = default)
    {
        return await RegisterAgentAsync(request, Roles.Operator, cancellationToken);
    }

    public async Task<Result<Guid>> RegisterAdminAsync(RegisterAgentRequest request,
        CancellationToken cancellationToken = default)
    {
        return await RegisterAgentAsync(request, Roles.Admin, cancellationToken);
    }

    private async Task<Result<Guid>> RegisterAgentAsync(RegisterAgentRequest request, string role,
        CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var actor = ActorFactory.CreateAgentActor();
        await _actorRepository.AddAsync(actor);

        var userRegisterIdentity = new UserRegisterIdentity
        (
            FirstName: request.FirstName,
            LastName: request.LastName,
            Email: request.Email,
            Role: role,
            ActorUser: actor,
            Password: request.Password
        );

        var userIdentityResult = await _identityService.CreateUserAsync(userRegisterIdentity);


        if (!userIdentityResult.IsSuccess)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result<Guid>.Failure(userIdentityResult.ErrorCode);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
        return Result<Guid>.Success(userIdentityResult.Value);
    }
}