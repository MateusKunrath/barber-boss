using BarberBoss.Communication.Enums;

namespace BarberBoss.Communication.Requests;

public class RequestGetBillingsJson : RequestFilteredJson
{
    public string? BarberName { get; set; }
    public string? ClientName { get; set; }
    public Status? Status { get; set; }
}