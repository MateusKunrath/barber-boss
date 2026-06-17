using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings;

public interface IBillingsReadOnlyRepository
{
    Task<List<Billing>> GetAll();
    Task<(List<Billing>, int)> GetAllFiltered(BillingFilters request);
    Task<Billing?> GetById(Guid id);
}