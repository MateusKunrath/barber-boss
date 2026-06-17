using BarberBoss.Domain.Enums;

namespace BarberBoss.Domain.Repositories.Billings;

public class GetBillingsFilters : BillingFilters
{
    public string? BarberName { get; set; }
    public string? ClientName { get; set; }
    public Status? Status { get; set; }
}