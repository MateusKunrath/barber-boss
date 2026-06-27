using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestUtilities.Requests;

public class RequestGetBillingsJsonBuilder
{
    public static RequestGetBillingsJson Build()
    {
        return new Faker<RequestGetBillingsJson>()
               .RuleFor(r => r.BarberName, _ => string.Empty)
               .RuleFor(r => r.ClientName, _ => string.Empty)
               .RuleFor(r => r.Status, _ => Status.Paid);
    }
}