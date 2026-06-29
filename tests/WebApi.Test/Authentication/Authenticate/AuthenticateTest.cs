using System.Globalization;
using System.Net;
using System.Text.Json;
using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Authentication.Authenticate;

public class AuthenticateTest : BarberBossClassFixture
{
    private const string Method = "api/Authenticate";

    private readonly string _email;
    private readonly string _name;
    private readonly string _password;

    public AuthenticateTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _email = webApplicationFactory.NormalUser.GetEmail();
        _name = webApplicationFactory.NormalUser.GetName();
        _password = webApplicationFactory.NormalUser.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestAuthenticateJson
        {
            Email = _email,
            Password = _password,
        };

        var response = await DoPost(Method, request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().Be(_name);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorAuthenticateInvalid(string culture)
    {
        var request = RequestAuthenticateJsonBuilder.Build();

        var response = await DoPost(Method, request, cultureInfo: culture);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}