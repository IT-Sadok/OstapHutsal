using System.Security.Claims;
using System.Text;
using CRMSystem.API.Common.ErrorMapping;
using CRMSystem.API.Endpoints;
using CRMSystem.API.Endpoints.Auth;
using CRMSystem.Application;
using CRMSystem.Application.Abstractions.Identity;
using CRMSystem.Application.Common.Authorization;
using CRMSystem.Infrastructure;
using CRMSystem.Infrastructure.Data;
using CRMSystem.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CRMSystem.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddApplication()
            .AddInfrastructure(builder.Configuration);

        builder.Services.AddSwaggerGen(options =>
        {
            // Add security definition for JWT Bearer
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter JWT token in the format: Bearer {token}"
            });

            // Require JWT for all endpoints (optional, you can also decorate specific endpoints)
            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>() // roles can be checked via Authorize
                }
            });
        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CrmDbContext>();
            await dbContext.Database.MigrateAsync();

            await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapAuthEndpoints();

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