using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Abstractions.Persistence.Repositories;

public interface ICommunicationChannelRepository : IGenericRepository<CommunicationChannel, Guid>
{
}