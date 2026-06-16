using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Billings;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class BillingsRepository(BarberBossDbContext dbContext) : IBillingsReadOnlyRepository, IBillingsWriteOnlyRepository
{
    public async Task<List<Billing>> GetAll()
    {
        return await dbContext.Billings.AsNoTracking().ToListAsync();
    }

    public async Task<Billing?> GetById(Guid id)
    {
        return await dbContext.Billings.AsNoTracking().FirstOrDefaultAsync(billing => billing.Id == id);
    }

    public async Task Add(Billing billing)
    {
        await dbContext.Billings.AddAsync(billing);
    }

    public async Task<bool> Delete(Guid id)
    {
        var result = await dbContext.Billings.FirstOrDefaultAsync(b => b.Id == id);
        if (result is null)
            return false;
        
        dbContext.Billings.Remove(result);
        return true;
    }
}