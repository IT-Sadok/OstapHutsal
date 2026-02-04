using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CRMSystem.Infrastructure.Data.Seeding;

public static class TicketCategorySeeder
{
    public static async Task SeedTicketCategoriesAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<CrmDbContext>();
        var ticketCategoryRepository = serviceProvider.GetRequiredService<ITicketCategoryRepository>();

        if (await context.TicketCategories.AnyAsync())
        {
            return;
        }

        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await ticketCategoryRepository.AddRangeAsync([
                new TicketCategory
                {
                    Type = TicketCategoryType.OrderManagement,
                    Name = "Orders",
                    Description = "Order creation, updates, and cancellations"
                },
                new TicketCategory
                {
                    Type = TicketCategoryType.ShippingAndDelivery,
                    Name = "Shipping & Delivery",
                    Description = "Delivery delays, courier issues, and tracking problems"
                },
                new TicketCategory
                {
                    Type = TicketCategoryType.ReturnsAndRefunds,
                    Name = "Returns & Refunds",
                    Description = "Product returns, exchanges, and refund requests"
                },
                new TicketCategory
                {
                    Type = TicketCategoryType.ProductIssues,
                    Name = "Product Issues",
                    Description = "Damaged, defective, or incorrect products"
                },
                new TicketCategory
                {
                    Type = TicketCategoryType.AccountAccess,
                    Name = "Account & Access",
                    Description = "Login problems, account recovery, and profile access"
                },
                new TicketCategory
                {
                    Type = TicketCategoryType.BillingAndPayments,
                    Name = "Billing & Payments",
                    Description = "Payment failures, invoices, and billing questions"
                },
                new TicketCategory
                {
                    Type = TicketCategoryType.TechnicalSupport,
                    Name = "Technical Support",
                    Description = "Website errors, system issues, and integrations"
                },
                new TicketCategory
                {
                    Type = TicketCategoryType.CustomerFeedback,
                    Name = "Customer Feedback",
                    Description = "Reviews, complaints, and service feedback"
                },
                new TicketCategory
                {
                    Type = TicketCategoryType.GeneralInquiry,
                    Name = "General Inquiry",
                    Description = "General questions and non-specific requests"
                },
                new TicketCategory
                {
                    Type = TicketCategoryType.FraudAndAbuse,
                    Name = "Fraud & Abuse",
                    Description = "Suspicious activity, fraud, or abuse reports"
                }
            ]);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}