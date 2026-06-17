using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Billings;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class BillingsRepository(BarberBossDbContext dbContext) : IBillingsReadOnlyRepository, IBillingsWriteOnlyRepository, IBillingsUpdateOnlyRepository
{
    public async Task<List<Billing>> GetAll()
    {
        return await dbContext.Billings.AsNoTracking().ToListAsync();
    }
    
    async Task<Billing?> IBillingsReadOnlyRepository.GetById(Guid id)
    {
        return await dbContext.Billings.AsNoTracking().FirstOrDefaultAsync(billing => billing.Id == id);
    }
    
    async Task<Billing?> IBillingsUpdateOnlyRepository.GetById(Guid id)
    {
        return await dbContext.Billings.FirstOrDefaultAsync(billing => billing.Id == id);
    }

    public void Update(Billing billing)
    {
        dbContext.Billings.Update(billing);
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