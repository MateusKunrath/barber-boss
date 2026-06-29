using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Infrastructure.DataAccess;
using BarberBoss.Infrastructure.Security.Tokens;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public UserIdentityManager NormalUser { get; set; } = null!;
    public UserIdentityManager AdminUser { get; set; } = null!;

    public BillingIdentityManager Billing { get; set; } = null!;

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

    private void StartDatabase(
        BarberBossDbContext dbContext,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        AddNormalUser(dbContext, passwordEncrypter, accessTokenGenerator);
        AddAdminUser(dbContext, passwordEncrypter, accessTokenGenerator);

        AddBilling(dbContext);

        dbContext.SaveChanges();
    }

    private User AddNormalUser(
        BarberBossDbContext dbContext,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Role = Role.User;
        var password = user.Password;
        user.Password = passwordEncrypter.Encrypt(user.Password);
        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        NormalUser = new UserIdentityManager(user, password, token);
        return user;
    }

    private User AddAdminUser(
        BarberBossDbContext dbContext,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Role = Role.Admin;
        var password = user.Password;
        user.Password = passwordEncrypter.Encrypt(user.Password);
        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        AdminUser = new UserIdentityManager(user, password, token);
        return user;
    }

    private Billing AddBilling(BarberBossDbContext dbContext)
    {
        var billing = BillingBuilder.Build();
        dbContext.Billings.Add(billing);

        Billing = new BillingIdentityManager(billing);

        return billing;
    }
}