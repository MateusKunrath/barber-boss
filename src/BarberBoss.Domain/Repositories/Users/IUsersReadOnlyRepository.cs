using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Users;

public interface IUsersReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
    Task<User?> GetUserByEmail(string email);
}