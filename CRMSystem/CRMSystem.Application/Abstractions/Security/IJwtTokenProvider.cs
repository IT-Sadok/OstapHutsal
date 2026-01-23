using Common;
using CRMSystem.Application.Auth.Models;
using CRMSystem.Application.Common;
using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Abstractions.Services;

public interface IJwtTokenProvider
{
    Result<string> Create(UserLoginIdentity userLoginIdentity);
}