using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace WebApi.Test.Billings.GetAll;

public class GetAllBillingsTest : BarberBossClassFixture
{
    private const string Method = "api/Billings";

    private readonly string _token;

    public GetAllBillingsTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.NormalUser.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(Method, _token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("billings").EnumerateArray().Should().NotBeNullOrEmpty();
    }
}