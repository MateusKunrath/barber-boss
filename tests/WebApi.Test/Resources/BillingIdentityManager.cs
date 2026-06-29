using BarberBoss.Domain.Entities;

namespace WebApi.Test.Resources;

public class BillingIdentityManager(Billing billing)
{
    public Guid GetId()
    {
        return billing.Id;
    }

    public DateTime GetDate()
    {
        return billing.Date;
    }
}