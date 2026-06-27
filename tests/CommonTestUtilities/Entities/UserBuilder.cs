using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using Bogus;
using CommonTestUtilities.Cryptography;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static User Build(Role role = Role.User)
    {
        var passwordEncrypter = new PasswordEncrypterBuilder().Build();

        return new Faker<User>()
               .RuleFor(user => user.Id, _ => Guid.NewGuid())
               .RuleFor(user => user.Name, faker => faker.Name.FullName())
               .RuleFor(user => user.Email, faker => faker.Internet.Email())
               .RuleFor(user => user.Password, (_, user) => passwordEncrypter.Encrypt(user.Password))
               .RuleFor(user => user.Role, _ => role);
    }
}