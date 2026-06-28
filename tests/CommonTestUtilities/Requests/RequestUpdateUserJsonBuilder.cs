using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Extensions;
using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
               .RuleFor(x => x.Name, faker => faker.Person.FullName)
               .RuleFor(x => x.Email, (faker, user) => faker.Internet.Email(user.Name))
               .RuleFor(x => x.Role, faker => faker.PickRandom<Role>().RoleToString());
    }
}