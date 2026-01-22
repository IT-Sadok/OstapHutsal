using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Infrastructure.Data;

namespace CRMSystem.Infrastructure.Repositories;

public class ClientRepository: GenericRepository<Client, Guid>, IClientRepository
{
    public ClientRepository(CrmDbContext context):base(context)
    {
        
    }
}