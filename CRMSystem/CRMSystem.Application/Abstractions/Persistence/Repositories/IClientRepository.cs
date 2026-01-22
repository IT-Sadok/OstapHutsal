using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Abstractions.Persistence.Repositories;

public interface IClientRepository: IGenericRepository<Client, Guid>
{
    
}