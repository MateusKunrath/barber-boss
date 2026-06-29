using System.Globalization;
using System.Net;
using System.Text.Json;
using BarberBoss.Domain.Enums;
using BarberBoss.Exception;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Billings.GetById;

public class GetBillingByIdTest : BarberBossClassFixture
{
    private const string Method = "api/Billings";

    private readonly Guid _billingId;
    private readonly string _token;

    public GetBillingByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _billingId = webApplicationFactory.Billing.GetId();
        _token = webApplicationFactory.NormalUser.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet($"{Method}/{_billingId.ToString()}", _token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetString().Should().Be(_billingId.ToString());
        response.RootElement.GetProperty("barberName").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("clientName").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("serviceName").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("notes").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(0);
        response.RootElement.GetProperty("date").GetDateTime().Should().NotBeAfter(DateTime.UtcNow);
        response.RootElement.GetProperty("createdAt").GetDateTime().Should().NotBeAfter(DateTime.UtcNow);
        response.RootElement.GetProperty("updatedAt").GetDateTime().Should().NotBeAfter(DateTime.UtcNow);

        var paymentMethod = response.RootElement.GetProperty("paymentMethod").GetInt32();
        Enum.IsDefined(typeof(PaymentMethod), paymentMethod).Should().BeTrue();

        var status = response.RootElement.GetProperty("status").GetInt32();
        Enum.IsDefined(typeof(Status), status).Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorBillingNotFound(string culture)
    {
        var result = await DoGet($"{Method}/{Guid.NewGuid().ToString()}", _token, culture);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessages =
            ResourceErrorMessages.ResourceManager.GetString("BILLING_NOT_FOUND", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessages));
    }
}