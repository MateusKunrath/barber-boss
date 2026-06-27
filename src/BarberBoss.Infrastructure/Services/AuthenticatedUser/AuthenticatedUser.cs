using System.IdentityModel.Tokens.Jwt;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Domain.Services.AuthenticatedUser;
using BarberBoss.Infrastructure.DataAccess;
using BarberBoss.Infrastructure.Security.Tokens;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.Services.AuthenticatedUser;

public class AuthenticatedUser(BarberBossDbContext dbContext, ITokenProvider tokenProvider) : IAuthenticatedUser
{
    public async Task<User> Get()
    {
        var token = tokenProvider.TokenOnRequest();
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type.Equals(CustomClaimTypes.Sid)).Value;
        return await dbContext
                     .Users
                     .AsNoTracking()
                     .FirstAsync(user => user.Id.Equals(Guid.Parse(identifier)));
    }
}