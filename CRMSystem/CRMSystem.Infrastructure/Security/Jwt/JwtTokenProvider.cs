using System.Security.Claims;
using System.Text;
using Common;
using CRMSystem.Application.Abstractions.Services;
using CRMSystem.Application.Auth;
using CRMSystem.Application.Auth.Models;
using CRMSystem.Application.Common;
using CRMSystem.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace CRMSystem.Infrastructure.Security.Jwt;

internal sealed class JwtTokenProvider : IJwtTokenProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }

    public Result<string> Create(UserLoginIdentity userLoginIdentity)
    {
        try
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, userLoginIdentity.UserId.ToString()),
                new Claim(nameof(userLoginIdentity.ActorId), userLoginIdentity.ActorId.ToString()),
            ];

            claims.AddRange(userLoginIdentity.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
                SigningCredentials = credentials,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
            };

            var tokenHandler = new JsonWebTokenHandler();

            string accessToken = tokenHandler.CreateToken(tokenDescriptor);
            return Result<string>.Success(accessToken);
        }
        catch
        {
            return Result<string>.Failure(AuthErrorCodes.JwtTokenGenerationFailed);
        }
    }
}