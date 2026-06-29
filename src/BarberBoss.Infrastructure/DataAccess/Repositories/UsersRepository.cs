using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class UsersRepository(BarberBossDbContext dbContext)
    : IUsersWriteOnlyRepository, IUsersReadOnlyRepository, IUsersUpdateOnlyRepository
{
    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await dbContext.Users.AnyAsync(user => user.Email.Equals(email));
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    async Task<User?> IUsersReadOnlyRepository.GetById(Guid id)
    {
        return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id.Equals(id));
    }

    async Task<User> IUsersUpdateOnlyRepository.GetById(Guid id)
    {
        return (await dbContext.Users.FirstOrDefaultAsync(user => user.Id.Equals(id)))!;
    }

    public void Update(User user)
    {
        dbContext.Users.Update(user);
    }

    public async Task Add(User user)
    {
        await dbContext.Users.AddAsync(user);
    }

    public async Task Delete(User user)
    {
        var userToRemove = await dbContext.Users.FindAsync(user.Id);
        dbContext.Users.Remove(userToRemove!);
    }
}