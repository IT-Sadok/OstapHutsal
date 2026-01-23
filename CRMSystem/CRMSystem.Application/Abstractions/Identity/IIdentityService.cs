using Common;
using CRMSystem.Application.Auth.Contracts;
using CRMSystem.Application.Auth.Models;
using CRMSystem.Application.Common;

namespace CRMSystem.Application.Abstractions.Identity;

public interface IIdentityService
{
    Task<Result<UserLoginIdentity>> AuthenticateAsync(LoginRequest loginRequest,
        CancellationToken cancellationToken = default);

    Task<Result<Guid>> CreateUserAsync(UserRegisterIdentity userRegisterIdentity);
    Task<Result> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
}