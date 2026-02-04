using Common;
using CRMSystem.Application.Common;
using CRMSystem.Application.Features.Auth.Contracts;
using CRMSystem.Application.Features.Auth.Models;

namespace CRMSystem.Application.Abstractions.Identity;

public interface IIdentityService
{
    Task<Result<UserLoginIdentity>> AuthenticateAsync(LoginRequest loginRequest,
        CancellationToken cancellationToken = default);

    Task<Result<Guid>> CreateUserAsync(UserRegisterIdentity userRegisterIdentity);
    Task<Result> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
}