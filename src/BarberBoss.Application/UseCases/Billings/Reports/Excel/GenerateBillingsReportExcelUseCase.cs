using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Billings;
using ClosedXML.Excel;

namespace BarberBoss.Application.UseCases.Billings.Reports.Excel;

public class GenerateBillingsReportExcelUseCase(IBillingsReadOnlyRepository repository)
    : IGenerateBillingsReportExcelUseCase
{
    private const string CurrencySymbol = "R$";

    public async Task<byte[]> Execute(DateOnly date)
    {
        var billings = await repository.FilterByDate(date);
        if (billings.Count == 0)
            return [];

        using var workbook = new XLWorkbook();

        workbook.Author = "Barber Boss";
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Arial";

        var worksheet = workbook.Worksheets.Add(date.ToString("Y").ToUpper());

        InsertHeader(worksheet);

        var raw = 2;
        foreach (var billing in billings)
        {
            worksheet.Cell($"A{raw}").Value = billing.BarberName;
            worksheet.Cell($"B{raw}").Value = billing.ClientName;
            worksheet.Cell($"C{raw}").Value = billing.ServiceName;
            
            worksheet.Cell($"D{raw}").Value = billing.Amount;
            worksheet.Cell($"D{raw}").Style.NumberFormat.Format = $"- {CurrencySymbol}#,##0.00";
            
            worksheet.Cell($"E{raw}").Value = billing.Notes;
            worksheet.Cell($"F{raw}").Value = billing.Date.ToString("d");
            worksheet.Cell($"G{raw}").Value = billing.PaymentMethod.PaymentMethodToString();
            worksheet.Cell($"H{raw}").Value = billing.Status.StatusToString();

            raw++;
        }
        
        worksheet.Columns().AdjustToContents();

        var file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private static void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessages.BARBER_NAME;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessages.CLIENT_NAME;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessages.SERVICE_NAME;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessages.NOTES;
        worksheet.Cell("F1").Value = ResourceReportGenerationMessages.DATE;
        worksheet.Cell("G1").Value = ResourceReportGenerationMessages.PAYMENT_METHOD;
        worksheet.Cell("H1").Value = ResourceReportGenerationMessages.STATUS;
        
        worksheet.Cells("A1:H1").Style.Font.Bold = true;
        worksheet.Cells("A1:H1").Style.Fill.BackgroundColor = XLColor.FromHtml("#f5c2b6");
        
        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("F1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }
}