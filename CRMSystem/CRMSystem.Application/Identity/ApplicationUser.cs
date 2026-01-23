using CRMSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CRMSystem.Application.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    public Guid ActorId { get; set; }
    public Actor Actor { get; set; } = null!;
}