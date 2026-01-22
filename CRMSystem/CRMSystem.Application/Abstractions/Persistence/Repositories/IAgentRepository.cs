using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Abstractions.Persistence.Repositories;

public interface IAgentRepository: IGenericRepository<Agent, Guid>
{
}