using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace BarberBoss.Infrastructure.Security.Tokens;

public class JwtTokenGenerator(uint expirationTimeInMinutes, string signingKey) : IAccessTokenGenerator
{
    public string Generate(User user)
    {
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.Sid, user.Id.ToString()),
            new(CustomClaimTypes.Name, user.Name),
            new(CustomClaimTypes.Role, user.Role.RoleToString()),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(expirationTimeInMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(claims),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    private SymmetricSecurityKey SecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(signingKey);
        return new SymmetricSecurityKey(key);
    }
}