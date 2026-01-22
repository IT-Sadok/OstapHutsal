using CRMSystem.Domain.Entities.Base;
using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities;

public class Actor: BaseEntity<Guid>
{
    public ActorKind Kind { get; internal set; }
    
    public Agent? Agent { get; internal set; }
    public Client? Client { get; internal set; }
    
    // ticket_message
    // ticket_history
    // attachment
}