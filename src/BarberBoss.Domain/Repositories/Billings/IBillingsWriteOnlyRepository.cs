using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings;

public interface IBillingsWriteOnlyRepository
{
    Task Add(Billing billing);
    
    /// <summary>
    ///     This function return "TRUE" if the deletion was successful otherwise returning "FALSE"
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> Delete(Guid id);
}