using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestUtilities.Requests;

public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build()
    {
        return new Faker<RequestRegisterUserJson>()
               .RuleFor(x => x.Name, faker => faker.Person.FullName)
               .RuleFor(x => x.Email, (faker, user) => faker.Internet.Email(user.Name))
               .RuleFor(x => x.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}