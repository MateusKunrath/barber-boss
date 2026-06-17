using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings;

public interface IBillingsReadOnlyRepository
{
    Task<List<Billing>> GetAll();
    Task<(List<Billing>, int)> GetAllFiltered(GetBillingsFilters request);
    Task<Billing?> GetById(Guid id);
}