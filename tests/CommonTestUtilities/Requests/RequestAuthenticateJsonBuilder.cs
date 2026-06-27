using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestUtilities.Requests;

public class RequestAuthenticateJsonBuilder
{
    public static RequestAuthenticateJson Build()
    {
        return new Faker<RequestAuthenticateJson>()
               .RuleFor(user => user.Email, faker => faker.Internet.Email())
               .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}