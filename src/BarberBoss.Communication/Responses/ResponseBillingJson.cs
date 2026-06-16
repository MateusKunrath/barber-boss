using BarberBoss.Communication.Enums;

namespace BarberBoss.Communication.Responses;

public class ResponseBillingJson
{
    public Guid Id { get; set; }
    public string BarberName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Notes { get; set; } = string.Empty;
    public PaymentMethod PaymentMethod { get; set; }
    public Status Status { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}