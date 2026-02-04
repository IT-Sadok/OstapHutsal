using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Abstractions.Persistence.Repositories;

public interface ITicketMessageRepository : IGenericRepository<TicketMessage, Guid>
{
}