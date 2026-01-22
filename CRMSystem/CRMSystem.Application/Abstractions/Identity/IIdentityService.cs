using CRMSystem.Application.Auth.Contracts;
using CRMSystem.Application.Auth.Models;
using CRMSystem.Application.Common;

namespace CRMSystem.Application.Abstractions.Identity;

public interface IIdentityService
{
    Task<Result<UserLoginIdentity>> AuthenticateAsync(LoginRequest loginRequest);
    Task<Result<Guid>> CreateUserAsync(UserRegisterIdentity userRegisterIdentity);
    Task<Result> DeleteUserAsync(Guid userId);
}