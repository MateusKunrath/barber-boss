namespace BarberBoss.Communication.Requests;

public class RequestGetBillingsJson
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; }
    public bool Descending { get; set; } = false;
}