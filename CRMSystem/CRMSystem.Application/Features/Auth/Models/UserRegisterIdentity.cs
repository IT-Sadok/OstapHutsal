using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Features.Auth.Models;

public record UserRegisterIdentity(
    string FirstName,
    string LastName,
    string Email,
    string Role,
    Actor ActorUser,
    string Password);