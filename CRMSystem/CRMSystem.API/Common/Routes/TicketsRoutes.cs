namespace CRMSystem.API.Common.Routes;

public static class TicketsRoutes
{
    public const string Base = "/tickets";

    public const string Root = ""; // GET /tickets
    public const string GetById = "/{ticketId}"; // GET /tickets/{ticketId}

    public const string CreateForCurrentClient = "/mine"; // POST /tickets/mine  (Client)
    public const string CreateForClient = "/clients/{clientId}"; // POST /tickets/clients/{clientId} (Agent)

    public const string Assignee = "/{ticketId}/assignee"; // PUT/DELETE
    public const string Status = "/{ticketId}/status";
    public const string Priority = "/{ticketId}/priority";
    public const string Category = "/{ticketId}/category";
}