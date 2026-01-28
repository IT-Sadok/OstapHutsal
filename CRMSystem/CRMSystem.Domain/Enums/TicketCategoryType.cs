namespace CRMSystem.Domain.Enums;

public enum TicketCategoryType
{
    Unknown = 0,

    OrderManagement = 1,
    ShippingAndDelivery = 2,
    ReturnsAndRefunds = 3,
    ProductIssues = 4,
    AccountAccess = 5,
    BillingAndPayments = 6,
    TechnicalSupport = 7,
    CustomerFeedback = 8,
    GeneralInquiry = 9,

    Custom = 90,
    FraudAndAbuse = 100
}