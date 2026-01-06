using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Infrastructure.Data;

public class CrmDbContext: DbContext
{
    public CrmDbContext(DbContextOptions<CrmDbContext> options):base(options)
    {
    }
}