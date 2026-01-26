using Common;
using CRMSystem.Application.Auth.Contracts;
using CRMSystem.Application.Common;

namespace CRMSystem.Application.Abstractions.Services;

public interface IAuthService
{
    Task<Result<string>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    Task<Result<Guid>> RegisterClientAsync(RegisterClientRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<Guid>> RegisterOperatorAsync(RegisterAgentRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<Guid>> RegisterAdminAsync(RegisterAgentRequest request, CancellationToken cancellationToken = default);
}