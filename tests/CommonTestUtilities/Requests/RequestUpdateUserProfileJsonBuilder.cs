using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserProfileJsonBuilder
{
    public static RequestUpdateUserProfileJson Build()
    {
        return new Faker<RequestUpdateUserProfileJson>()
               .RuleFor(user => user.Name, faker => faker.Person.FullName)
               .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name));
    }
}