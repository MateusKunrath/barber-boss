using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Services.AuthenticatedUser;

public interface IAuthenticatedUser
{
    Task<User> Get();
}