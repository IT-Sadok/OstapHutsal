using CRMSystem.Application.Common.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CRMSystem.Infrastructure.Identity;

public class ApplicationRole: IdentityRole<Guid>
{
    public ApplicationRole() { }
    public ApplicationRole(string roleName) : base(roleName)
    {
    }
}