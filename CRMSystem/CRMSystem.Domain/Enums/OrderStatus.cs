namespace CRMSystem.Domain.Enums;

public enum OrderStatus
{
    Created = 1,
    PendingPayment,
    Paid,
    Processing,
    Shipped,
    Delivered,
    Cancelled,
    Returned,
    Refunded
}