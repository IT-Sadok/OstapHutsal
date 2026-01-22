namespace CRMSystem.Application.Auth.Contracts;

public record RegisterClientRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? Phone = null,
    string? Address = null
);