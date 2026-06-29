using System.Globalization;
using System.Net;
using System.Text.Json;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Billings.Register;

public class RegisterBillingTest : BarberBossClassFixture
{
    private const string Method = "api/Billings";

    private readonly string _token;

    public RegisterBillingTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.NormalUser.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestBillingJsonBuilder.Build();

        var result = await DoPost(Method, request, _token);
        result.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorBarberNameEmpty(string culture)
    {
        var request = RequestBillingJsonBuilder.Build();
        request.BarberName = string.Empty;

        var result = await DoPost(Method, request, _token, culture);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("BARBER_NAME_REQUIRED", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}