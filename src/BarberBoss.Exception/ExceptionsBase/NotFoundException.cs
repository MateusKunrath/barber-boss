using System.Net;

namespace BarberBoss.Exception.ExceptionsBase;

public class NotFoundException(string message) : BarberBossException(message)
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;
    
    public override List<string> GetErrors()
    {
        return [Message];
    }
}