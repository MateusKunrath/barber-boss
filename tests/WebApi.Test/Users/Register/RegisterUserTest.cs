using System.Globalization;
using System.Net;
using System.Text.Json;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Register;

public class RegisterUserTest(CustomWebApplicationFactory webApplicationFactory)
    : BarberBossClassFixture(webApplicationFactory)
{
    private const string Method = "api/Users";

    [Fact]
    public async Task Success()
    {
        var request = RequestUserJsonBuilder.Build();

        var result = await DoPost(Method, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorNameEmpty(string cultureInfo)
    {
        var request = RequestUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPost(Method, request, cultureInfo: cultureInfo);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(cultureInfo));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}