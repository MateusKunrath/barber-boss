using BarberBoss.Domain.Entities;

namespace BarberBoss.Infrastructure.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}