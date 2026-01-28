using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Abstractions.Persistence.Repositories;

public interface ITicketRepository : IGenericRepository<Ticket, Guid>
{
}