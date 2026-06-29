using BarberBoss.Domain.Entities;

namespace WebApi.Test.Resources;

public class UserIdentityManager(User user, string password, string token)
{
    public string GetName()
    {
        return user.Name;
    }

    public string GetEmail()
    {
        return user.Email;
    }

    public string GetPassword()
    {
        return user.Password;
    }

    public string GetToken()
    {
        return token;
    }

    public Guid GetId()
    {
        return user.Id;
    }
}