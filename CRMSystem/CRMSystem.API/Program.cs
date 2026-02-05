using CRMSystem.API.Common.ErrorMapping;
using CRMSystem.API.Common.Extensions;
using CRMSystem.Application;
using CRMSystem.Application.Abstractions.Identity;
using CRMSystem.Infrastructure;

namespace CRMSystem.API;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureSignalR();
        builder.Services.AddValidation();
        
        builder.Services.AddApplication()
            .AddInfrastructure(builder.Configuration);

        builder.Services.ConfigureSwagger();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            await app.ApplyMigrationsAndSeedAsync();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapEndpoints();

        app.MapSignalRHubs();

        // temporary endpoint
        app.MapDelete("/resources/delete", async (
            IIdentityService identityService,
            string id
        ) =>
        {
            var result = await identityService.DeleteUserAsync(Guid.Parse(id));
            return result.IsSuccess
                ? Results.NoContent()
                : AuthErrorMapper.ToHttpResult(result.ErrorCode);
        });

        await app.RunAsync();
    }
}