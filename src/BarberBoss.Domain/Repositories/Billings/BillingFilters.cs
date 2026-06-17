namespace BarberBoss.Domain.Repositories.Billings;

public class BillingFilters
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; }
    public bool Descending { get; set; }
}