namespace CRMSystem.Application.SignalR.Protocol;

public readonly record struct ClientEvent(string Name)
{
    public override string ToString() => Name;

    public static readonly ClientEvent TicketAssigned =
        new(ClientEvents.TicketAssigned);

    public static readonly ClientEvent TicketUnassigned =
        new(ClientEvents.TicketUnassigned);
}