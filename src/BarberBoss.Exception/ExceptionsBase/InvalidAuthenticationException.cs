using System.Net;

namespace BarberBoss.Exception.ExceptionsBase;

public class InvalidAuthenticationException() : BarberBossException(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID)
{
    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}