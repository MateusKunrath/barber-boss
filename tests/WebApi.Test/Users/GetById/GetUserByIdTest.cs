using System.Globalization;
using System.Net;
using System.Text.Json;
using BarberBoss.Communication.Enums;
using BarberBoss.Exception;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.GetById;

public class GetUserByIdTest : BarberBossClassFixture
{
    private const string Method = "api/Users";

    private readonly string _token;
    private readonly string _userEmail;
    private readonly Guid _userId;
    private readonly string _userName;

    public GetUserByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.NormalUser.GetToken();
        _userEmail = webApplicationFactory.NormalUser.GetEmail();
        _userId = webApplicationFactory.NormalUser.GetId();
        _userName = webApplicationFactory.NormalUser.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet($"{Method}/{_userId.ToString()}", _token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetGuid().Should().Be(_userId);
        response.RootElement.GetProperty("name").GetString().Should().Be(_userName);
        response.RootElement.GetProperty("email").GetString().Should().Be(_userEmail);
        response.RootElement.GetProperty("createdAt").GetDateTime().Should().NotBeAfter(DateTime.UtcNow);
        response.RootElement.GetProperty("updatedAt").GetDateTime().Should().NotBeAfter(DateTime.UtcNow);

        var role = response.RootElement.GetProperty("role").GetString();
        Enum.IsDefined(typeof(Role), Enum.Parse<Role>(role!)).Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorUserNotFound(string culture)
    {
        var result = await DoGet($"{Method}/{Guid.NewGuid().ToString()}", _token, culture);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("USER_NOT_FOUND", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}