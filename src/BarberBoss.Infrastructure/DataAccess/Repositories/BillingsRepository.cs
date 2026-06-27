using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Repositories.Billings;
using Microsoft.EntityFrameworkCore;
using BarberBoss.Infrastructure.DataAccess.Extensions;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class BillingsRepository(BarberBossDbContext dbContext) : IBillingsReadOnlyRepository, IBillingsWriteOnlyRepository, IBillingsUpdateOnlyRepository
{
    public async Task<List<Billing>> GetAll()
    {
        return await dbContext.Billings.AsNoTracking().ToListAsync();
    }

    public async Task<(List<Billing>, int)> GetAllFiltered(GetBillingsFilters request)
    {
        var query = dbContext.Billings.AsNoTracking().AsQueryable();
        
        query = query.ApplyFilters(request);

        query = request.OrderBy switch
        {
            "date" => request.Descending ? query.OrderByDescending(b => b.Date) : query.OrderBy(b => b.Date),
            "amount" => request.Descending ? query.OrderByDescending(b => b.Amount) : query.OrderBy(b => b.Amount),
            _ => query.OrderBy(b => b.Date)
        };
        
        var totalCount = await query.CountAsync();

        var billings = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        
        return (billings, totalCount);
    }

    async Task<Billing?> IBillingsReadOnlyRepository.GetById(Guid id)
    {
        return await dbContext.Billings.AsNoTracking().FirstOrDefaultAsync(billing => billing.Id == id);
    }

    public async Task<List<Billing>> FilterByDate(DateOnly date)
    {
        var startDate = new DateTime(date.Year, date.Month, 1).Date;
        
        var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        var endDate = new DateTime(date.Year, date.Month, daysInMonth, 23, 59, 59);
        
        return await dbContext
            .Billings
            .AsNoTracking()
            .Where(billing => billing.Date >= startDate && billing.Date <= endDate)
            .OrderBy(billing => billing.Date)
            .ThenBy(billing => billing.BarberName)
            .ToListAsync();
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

    public async Task Delete(Guid id)
    {
        var result = await dbContext.Billings.FindAsync(id);
        dbContext.Billings.Remove(result!);
    }
}