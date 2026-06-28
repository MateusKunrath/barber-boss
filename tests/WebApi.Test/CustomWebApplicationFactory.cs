using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Infrastructure.DataAccess;
using BarberBoss.Infrastructure.Security.Tokens;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests")
               .ConfigureServices(services =>
               {
                   var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                   services.AddDbContext<BarberBossDbContext>(config =>
                   {
                       config.UseInMemoryDatabase("InMemoryDbForTesting");
                       config.UseInternalServiceProvider(provider);
                   });

                   var scope = services.BuildServiceProvider().CreateScope();
                   var dbContext = scope.ServiceProvider.GetRequiredService<BarberBossDbContext>();
                   var passwordEncrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncrypter>();
                   var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                   StartDatabase(dbContext, passwordEncrypter, accessTokenGenerator);
               });
    }

    private void StartDatabase(BarberBossDbContext dbContext, IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator accessTokenGenerator) { }
}