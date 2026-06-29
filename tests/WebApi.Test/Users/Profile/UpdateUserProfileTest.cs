using System.Globalization;
using System.Net;
using System.Text.Json;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Profile;

public class UpdateUserProfileTest : BarberBossClassFixture
{
    private const string Method = "api/Users";

    private readonly string _token;

    public UpdateUserProfileTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.NormalUser.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();

        var response = await DoPut(Method, request, _token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorEmptyName(string culture)
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await DoPut(Method, request, _token, culture);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}