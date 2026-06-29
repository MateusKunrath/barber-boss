using System.Net;
using System.Net.Mime;
using FluentAssertions;

namespace WebApi.Test.Billings.Reports;

public class GenerateBillingsReportTest : BarberBossClassFixture
{
    private const string Method = "api/Reports";

    private readonly string _adminToken;
    private readonly DateTime _billingDate;
    private readonly string _userToken;

    public GenerateBillingsReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.AdminUser.GetToken();
        _userToken = webApplicationFactory.NormalUser.GetToken();
        _billingDate = webApplicationFactory.Billing.GetDate();
    }

    [Fact]
    public async Task SuccessPdf()
    {
        var result = await DoGet($"{Method}/Pdf?month={_billingDate:yyyy-MM}", _adminToken);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task SuccessExcel()
    {
        var result = await DoGet($"{Method}/Excel?month={_billingDate:yyyy-MM}", _adminToken);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task ErrorForbiddenUserNotAllowedExcel()
    {
        var result = await DoGet($"{Method}/Excel?month={_billingDate:yyyy-MM}", _userToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}