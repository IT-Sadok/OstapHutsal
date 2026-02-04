using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Infrastructure.Data;

namespace CRMSystem.Infrastructure.Repositories;

public class CommunicationChannelRepository : GenericRepository<CommunicationChannel, Guid>,
    ICommunicationChannelRepository
{
    public CommunicationChannelRepository(CrmDbContext context) : base(context)
    {
    }
}