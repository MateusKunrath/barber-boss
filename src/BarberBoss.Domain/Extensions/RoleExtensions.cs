using BarberBoss.Domain.Enums;

namespace BarberBoss.Domain.Extensions;

public static class RoleExtensions
{
    public static string RoleToString(this Role role)
    {
        return role switch
        {
            Role.User => nameof(Role.User),
            Role.Admin => nameof(Role.Admin),
            _ => string.Empty,
        };
    }
}