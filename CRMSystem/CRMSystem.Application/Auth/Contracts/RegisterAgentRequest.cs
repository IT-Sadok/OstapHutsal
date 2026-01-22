namespace CRMSystem.Application.Auth.Contracts;

public record RegisterAgentRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName
);