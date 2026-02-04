namespace CRMSystem.Application.Common.Authorization;

public static class Policies
{
    public const string Admin = "Admin";
    public const string SuperAdmin = "SuperAdmin";
    public const string Client = "Client";
    public const string Operator = "Operator";

    public const string OperatorOrAdmin = "OperatorOrAdmin";
}