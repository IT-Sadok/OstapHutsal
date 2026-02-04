using System.Security.Claims;

namespace CRMSystem.Application.Common.Security;

public interface IUserContextProvider
{
    UserContext? FromHttpContext();
}