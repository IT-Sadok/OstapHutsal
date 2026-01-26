using CRMSystem.Domain.Entities.Base;

namespace CRMSystem.Domain.Entities;

public class Client: BaseEntity<Guid>
{
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? ExternalId { get; set; }
    public int? Rating { get; set; }
    
    public Guid ActorId { get; set; }
    public Actor Actor { get; set; } = null!;
    
    // order
    // ticket
}

