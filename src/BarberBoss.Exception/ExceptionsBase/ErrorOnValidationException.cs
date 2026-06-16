using System.Net;

namespace BarberBoss.Exception.ExceptionsBase;

public class ErrorOnValidationException(List<string> errorMessages) : BarberBossException(string.Empty)
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors()
    {
        return errorMessages;
    }
}