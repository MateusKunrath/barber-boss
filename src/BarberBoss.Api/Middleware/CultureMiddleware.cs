using System.Globalization;

namespace BarberBoss.Api.Middleware;

public class CultureMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext httpContext)
    {
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
        var requestedCulture = httpContext.Request.Headers.AcceptLanguage.FirstOrDefault();
        
        var cultureInfo = new CultureInfo("en");

        if (
            !string.IsNullOrWhiteSpace(requestedCulture)
            && supportedLanguages.Exists(l => l.Name.Equals(requestedCulture))
        )
        {
            cultureInfo = new CultureInfo(requestedCulture);
        }
        
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
        
        await next(httpContext);
    }
}