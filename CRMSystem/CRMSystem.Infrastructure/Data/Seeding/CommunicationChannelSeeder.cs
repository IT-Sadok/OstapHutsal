using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CRMSystem.Infrastructure.Data.Seeding
{
    public static class CommunicationChannelSeeder
    {
        public static async Task SeedChannelsAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<CrmDbContext>();
            var channelRepository = serviceProvider.GetRequiredService<ICommunicationChannelRepository>();

            if (await context.Channels.AnyAsync())
                return;

            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var channels = Enum.GetValues<ChannelType>()
                    .Select(ct => new CommunicationChannel
                    {
                        ChannelType = ct,
                        Description = ct switch
                        {
                            ChannelType.WebsiteForm => "Submitted via website form",
                            ChannelType.Email => "Submitted via email",
                            ChannelType.Phone => "Phone call",
                            ChannelType.Chat => "Live chat",
                            ChannelType.Telegram => "Telegram bot",
                            ChannelType.Facebook => "Facebook page",
                            ChannelType.Instagram => "Instagram page",
                            _ => null
                        }
                    })
                    .ToList();

                await channelRepository.AddRangeAsync(channels);
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
}