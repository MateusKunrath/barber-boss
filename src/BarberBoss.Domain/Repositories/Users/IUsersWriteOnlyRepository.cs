using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Users;

public interface IUsersWriteOnlyRepository
{
    Task Add(User user);
    Task Delete(User user);
}