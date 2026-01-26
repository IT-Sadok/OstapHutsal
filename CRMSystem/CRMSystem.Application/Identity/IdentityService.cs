using Common;
using CRMSystem.Application.Abstractions.Identity;
using CRMSystem.Application.Abstractions.Persistence;
using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Application.Auth;
using CRMSystem.Application.Auth.Contracts;
using CRMSystem.Application.Auth.Models;
using CRMSystem.Application.Common;
using CRMSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Application.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IActorRepository _actorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public IdentityService(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IActorRepository actorRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _actorRepository = actorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserLoginIdentity>> AuthenticateAsync(LoginRequest loginRequest,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .Include(u => u.Actor)
            .ThenInclude(a => a.Agent)
            .Include(u => u.Actor)
            .ThenInclude(a => a.Client)
            .Where(u => u.Email == loginRequest.Email)
            .Where(u => (u.Actor.Kind == ActorKind.Agent && u.Actor.Agent != null &&
                         u.Actor.Agent.Status == AgentStatus.Active)
                        || (u.Actor.Kind == ActorKind.Client && u.Actor.Client != null))
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result<UserLoginIdentity>.Failure(AuthErrorCodes.UserInvalid);

        if (await _userManager.IsLockedOutAsync(user))
            return Result<UserLoginIdentity>.Failure(AuthErrorCodes.UserLocked);

        if (!await _userManager.CheckPasswordAsync(user, loginRequest.Password))
        {
            await _userManager.AccessFailedAsync(user);
            return Result<UserLoginIdentity>.Failure(AuthErrorCodes.InvalidPassword);
        }

        await _userManager.ResetAccessFailedCountAsync(user);

        var roles = await _userManager.GetRolesAsync(user);

        return Result<UserLoginIdentity>.Success(
            new UserLoginIdentity(
                user.Id,
                user.ActorId,
                roles.AsReadOnly()
            )
        );
    }

    public async Task<Result<Guid>> CreateUserAsync(UserRegisterIdentity userRegisterIdentity)
    {
        var userExists = await _userManager.FindByEmailAsync(userRegisterIdentity.Email);
        if (userExists is not null)
        {
            return Result<Guid>.Failure(AuthErrorCodes.EmailAlreadyExists);
        }

        var roleExists = await _roleManager.RoleExistsAsync(userRegisterIdentity.Role);
        if (!roleExists)
        {
            return Result<Guid>.Failure(AuthErrorCodes.RoleNotFound);
        }

        var user = new ApplicationUser
        {
            UserName = userRegisterIdentity.Email,
            FirstName = userRegisterIdentity.FirstName,
            LastName = userRegisterIdentity.LastName,
            Email = userRegisterIdentity.Email,
            EmailConfirmed = true, // remove after implementation of email confirmation
            Actor = userRegisterIdentity.ActorUser,
        };

        var userCreateResult = await _userManager.CreateAsync(user, userRegisterIdentity.Password);
        if (!userCreateResult.Succeeded)
        {
            return Result<Guid>.Failure(AuthErrorCodes.UserCreationFailed);
        }

        var roleAssignResult = await _userManager.AddToRoleAsync(user, userRegisterIdentity.Role);
        if (!roleAssignResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            return Result<Guid>.Failure(AuthErrorCodes.AssigningRoleFailed);
        }

        return Result<Guid>.Success(user.Id);
    }

    public async Task<Result> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .Include(u => u.Actor)
            .ThenInclude(a => a.Agent)
            .Include(u => u.Actor)
            .ThenInclude(a => a.Client)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            return Result.Failure(AuthErrorCodes.InvalidId);

        _actorRepository.Remove(user.Actor);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}