using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Reports;

namespace BarberBoss.Domain.Extensions;

public static class StatusExtensions
{
    public static string StatusToString(this Status status)
    {
        return status switch
        {
            Status.Paid => ResourceReportGenerationMessages.STATUS_PAID,
            Status.Cancelled => ResourceReportGenerationMessages.STATUS_CANCELLED,
            _ => string.Empty
        };
    }
}