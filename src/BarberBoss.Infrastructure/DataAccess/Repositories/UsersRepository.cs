using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class UsersRepository(BarberBossDbContext dbContext) : IUsersWriteOnlyRepository, IUsersReadOnlyRepository
{
    public async Task Add(User user)
    {
        await dbContext.Users.AddAsync(user);
    }
    
    public async Task Delete(User user)
    {
        var userToRemove = await dbContext.Users.FindAsync(user);
        dbContext.Users.Remove(userToRemove!);
    }
    
    public async Task<User?> GetUserByEmail(string email)
    {
        return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }
}