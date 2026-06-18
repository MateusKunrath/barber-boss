using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestUtilities.Requests;

public class RequestRegisterBillingJsonBuilder
{
    public static RequestBillingJson Build()
    {
        return new Faker<RequestBillingJson>()
            .RuleFor(r => r.BarberName, faker => faker.Name.FullName())
            .RuleFor(r => r.ClientName, faker => faker.Name.FullName())
            .RuleFor(r => r.ServiceName, faker => faker.Commerce.ProductName())
            .RuleFor(r => r.Date, faker => faker.Date.Past())
            .RuleFor(r => r.PaymentMethod, faker => faker.PickRandom<PaymentMethod>())
            .RuleFor(r => r.Status, Status.Paid)
            .RuleFor(r => r.Amount, faker => faker.Random.Decimal(1, 1000));
    }
}