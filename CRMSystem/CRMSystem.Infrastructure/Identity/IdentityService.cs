using CRMSystem.Application.Abstractions.Identity;
using CRMSystem.Application.Auth;
using CRMSystem.Application.Auth.Contracts;
using CRMSystem.Application.Auth.Models;
using CRMSystem.Application.Common;
using CRMSystem.Domain.Enums;
using CRMSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly CrmDbContext _context;

    public IdentityService(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        CrmDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<Result<UserLoginIdentity>> AuthenticateAsync(LoginRequest loginRequest)
    {
        var user = await _userManager.Users
            .Include(u => u.Actor)
            .ThenInclude(a => a.Agent)
            .Include(u => u.Actor)
            .ThenInclude(a => a.Client)
            .FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

        if (user is null)
            return Result<UserLoginIdentity>.Failure(AuthErrorCodes.InvalidEmail);

        var actor = user.Actor;

        if (actor.Kind == ActorKind.Agent && (actor.Agent is null || !actor.Agent.IsActive))
            return Result<UserLoginIdentity>.Failure(AuthErrorCodes.UserInactive);

        if (actor is { Kind: ActorKind.Client, Client: null })
            return Result<UserLoginIdentity>.Failure(AuthErrorCodes.UserInactive);

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

    public async Task<Result> DeleteUserAsync(Guid userId)
    {
        var user = await _userManager.Users
            .Include(u => u.Actor)
            .ThenInclude(a => a.Agent)
            .Include(u => u.Actor)
            .ThenInclude(a => a.Client)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return Result.Failure(AuthErrorCodes.InvalidId);

        _context.Actors.Remove(user.Actor);
        await _context.SaveChangesAsync();

        return Result.Success();
    }
}