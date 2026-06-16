using BarberBoss.Domain.Repositories;

namespace BarberBoss.Infrastructure.DataAccess;

public class UnitOfWork(BarberBossDbContext dbContext) : IUnitOfWork
{
    public async Task Commit()
    {
        await dbContext.SaveChangesAsync();
    }
}