using System.Globalization;
using System.Net;
using System.Text.Json;
using BarberBoss.Exception;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Billings.Delete;

public class DeleteBillingTest : BarberBossClassFixture
{
    private const string Method = "api/Billings";

    private readonly Guid _billingId;
    private readonly string _token;


    public DeleteBillingTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _billingId = webApplicationFactory.Billing.GetId();
        _token = webApplicationFactory.NormalUser.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete($"{Method}/{_billingId.ToString()}", _token);
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        result = await DoGet($"{Method}/{_billingId.ToString()}", _token);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorBillingNotFound(string culture)
    {
        var result = await DoDelete($"{Method}/{Guid.NewGuid()}", _token, culture);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("BILLING_NOT_FOUND", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}