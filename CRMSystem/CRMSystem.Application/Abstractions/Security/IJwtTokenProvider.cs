using Common;
using CRMSystem.Application.Features.Auth.Models;

namespace CRMSystem.Application.Abstractions.Security;

public interface IJwtTokenProvider
{
    Result<string> Create(UserLoginIdentity userLoginIdentity);
}