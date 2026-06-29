using System.Globalization;
using System.Net;
using System.Text.Json;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Billings.Update;

public class UpdateBillingTest : BarberBossClassFixture
{
    private const string Method = "api/Billings";

    private readonly Guid _billingId;
    private readonly string _token;

    public UpdateBillingTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.NormalUser.GetToken();
        _billingId = webApplicationFactory.Billing.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestBillingJsonBuilder.Build();

        var result = await DoPut($"{Method}/{_billingId.ToString()}", request, _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorBarberNameEmpty(string culture)
    {
        var request = RequestBillingJsonBuilder.Build();
        request.BarberName = string.Empty;

        var result = await DoPut($"{Method}/{_billingId.ToString()}", request, _token, culture);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("BARBER_NAME_REQUIRED", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorBarberNameNotFound(string culture)
    {
        var request = RequestBillingJsonBuilder.Build();

        var result = await DoPut($"{Method}/{Guid.NewGuid().ToString()}", request, _token, culture);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("BILLING_NOT_FOUND", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}