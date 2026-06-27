using System.IdentityModel.Tokens.Jwt;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Infrastructure.DataAccess;
using BarberBoss.Infrastructure.Security.Tokens;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.Services.LoggedUser;

public class LoggedUser(BarberBossDbContext dbContext, ITokenProvider tokenProvider) : ILoggedUser
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