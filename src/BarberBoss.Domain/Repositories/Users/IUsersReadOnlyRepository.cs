using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Users;

public interface IUsersReadOnlyRepository
{
    Task<User?> GetUserByEmail(string email);
}