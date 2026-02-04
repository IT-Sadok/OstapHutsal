using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CRMSystem.API.SignalR.Hubs;

[Authorize]
public class NotificationsHub : Hub
{
}