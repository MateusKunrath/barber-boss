using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using Bogus;

namespace CommonTestUtilities.Entities;

public class BillingBuilder
{
    public static List<Billing> Collection(uint count = 2)
    {
        var list = new List<Billing>();

        if (count == 0)
        {
            count = 1;
        }

        for (int i = 0; i < count; i++)
        {
            var billing = Build();
            list.Add(billing);
        }

        return list;
    }

    public static Billing Build()
    {
        return new Faker<Billing>()
               .RuleFor(b => b.Id, faker => faker.Random.Guid())
               .RuleFor(b => b.BarberName, faker => faker.Person.FullName)
               .RuleFor(b => b.ClientName, faker => faker.Person.FullName)
               .RuleFor(b => b.ServiceName, faker => faker.Commerce.ProductName())
               .RuleFor(b => b.Notes, faker => faker.Lorem.Sentence())
               .RuleFor(b => b.Date, faker => faker.Date.Past())
               .RuleFor(b => b.Amount, faker => faker.Random.Decimal(1, 1000))
               .RuleFor(b => b.PaymentMethod, faker => faker.PickRandom<PaymentMethod>())
               .RuleFor(b => b.Status, _ => Status.Paid)
               .RuleFor(b => b.CreatedAt, _ => DateTime.UtcNow)
               .RuleFor(b => b.UpdatedAt, _ => DateTime.UtcNow);
    }
}