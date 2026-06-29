using System.Globalization;
using System.Net;
using System.Text.Json;
using BarberBoss.Exception;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Delete;

public class DeleteUserTest : BarberBossClassFixture
{
    private const string Method = "api/Users";

    private readonly string _token;
    private readonly Guid _userId;

    public DeleteUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.NormalUser.GetToken();
        _userId = webApplicationFactory.NormalUser.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete($"{Method}/{_userId.ToString()}", _token);
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorUserNotFound(string culture)
    {
        var result = await DoDelete($"{Method}/{Guid.NewGuid().ToString()}", _token, culture);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("USER_NOT_FOUND", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}