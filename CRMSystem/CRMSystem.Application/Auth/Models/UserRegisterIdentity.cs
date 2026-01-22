using CRMSystem.Application.Common.Authorization;
using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Auth.Models;

public record UserRegisterIdentity(
    string FirstName,
    string LastName,
    string Email,
    string Role,
    Actor ActorUser,
    string Password);