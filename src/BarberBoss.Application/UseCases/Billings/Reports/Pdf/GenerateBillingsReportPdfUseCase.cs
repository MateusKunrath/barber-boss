using System.Reflection;
using BarberBoss.Application.UseCases.Billings.Reports.Pdf.Colors;
using BarberBoss.Application.UseCases.Billings.Reports.Pdf.Fonts;
using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Billings;
using DocumentFormat.OpenXml.Packaging;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace BarberBoss.Application.UseCases.Billings.Reports.Pdf;

public class GenerateBillingsReportPdfUseCase : IGenerateBillingsReportPdfUseCase
{
    private const string CurrencySymbol = "R$";
    private const int HeightRowBillingTable = 25;
    
    private readonly IBillingsReadOnlyRepository _repository;
    
    public GenerateBillingsReportPdfUseCase(IBillingsReadOnlyRepository repository)
    {
        _repository = repository;
        GlobalFontSettings.FontResolver = new BillingsReportFontResolver();
    }
    
    public async Task<byte[]> Execute(DateOnly date)
    {
        var billings = await _repository.FilterByDate(date);
        if (billings.Count == 0)
            return [];

        var document = CreateDocument(date);
        var page = CreatePage(document);
        
        CreateHeaderWithLogoAndName(page);
        
        var totalBillings = billings.Sum(b => b.Amount);
        CreateTotalBillingSection(page, date, totalBillings);

        foreach (var billing in billings)
        {
            var table = CreateBillingTable(page);
            
            var row = table.AddRow();
            row.Height = HeightRowBillingTable;
            
            AddHeaderForBillingServiceName(row.Cells[0], billing.ServiceName);
            AddHeaderForBillingAmount(row.Cells[3]);
            
            row = table.AddRow();
            row.Height = HeightRowBillingTable;

            row.Cells[0].AddParagraph(billing.Date.ToString("D"));
            row.Cells[0].Format.LeftIndent = 9;
            SetStyleBaseForBillingInformation(row.Cells[0]);

            row.Cells[1].AddParagraph(billing.Date.ToString("t"));
            SetStyleBaseForBillingInformation(row.Cells[1]);

            row.Cells[2].AddParagraph(billing.PaymentMethod.PaymentMethodToString());
            SetStyleBaseForBillingInformation(row.Cells[2]);
            
            AddAmountForBilling(row.Cells[3], billing.Amount);

            if (!string.IsNullOrWhiteSpace(billing.Notes))
            {
                var notesRow = table.AddRow();
                notesRow.Height = HeightRowBillingTable;
                
                notesRow.Cells[0].AddParagraph(billing.Notes);
                notesRow.Cells[0].Format.Font = new Font
                {
                    Name = FontsHelper.RobotoRegular,
                    Size = 9,
                    Color = ColorsHelper.Black,
                };
                notesRow.Cells[0].Shading.Color = ColorsHelper.Green100;
                notesRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                notesRow.Cells[0].MergeRight = 2;
                notesRow.Cells[0].Format.LeftIndent = 7;

                row.Cells[3].MergeDown = 1;
            }

            AddWhiteSpace(table);
        }
        
        return RenderDocument(document);
    }

    private static Document CreateDocument(DateOnly date)
    {
        var document = new Document
        {
            Info =
            {
                Title = $"{ResourceReportGenerationMessages.BILLINGS_FOR} {date:Y}",
                Author = "Barber Boss",
            }
        };
        
        var style = document.Styles["Normal"];
        style!.Font.Name = FontsHelper.BebasNeueRegular;
        return document;
    }

    private static Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 35;
        section.PageSetup.RightMargin = 35;
        section.PageSetup.TopMargin = 53;
        section.PageSetup.BottomMargin = 53;

        return section;
    }

    private static void CreateHeaderWithLogoAndName(Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");
        
        var row = table.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "BarberBossLogo.png");
        
        row.Cells[0].AddImage(pathFile);

        row.Cells[1].AddParagraph("Barbearia do Souza");
        row.Cells[1].Format.Font = new Font { Name = FontsHelper.BebasNeueRegular, Size = 25 };
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }

    private static void CreateTotalBillingSection(Section page, DateOnly date, decimal totalBillings)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "38";
        paragraph.Format.SpaceAfter = "64";

        var title = string.Format(ResourceReportGenerationMessages.TOTAL_BILLINGS_ON, date.ToString("Y"));

        paragraph.AddFormattedText(title, new Font { Name = FontsHelper.RobotoMedium, Size = 15 });
        
        paragraph.AddLineBreak();

        paragraph.AddFormattedText(
            $"{CurrencySymbol} {totalBillings}",
            new Font { Name = FontsHelper.BebasNeueRegular, Size = 50 }
        );
    }

    private static Table CreateBillingTable(Section page)
    {
        var table = page.AddTable();
        
        table.AddColumn("205").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

    private static void AddHeaderForBillingServiceName(Cell cell, string billingServiceName)
    {
        cell.AddParagraph(billingServiceName);
        cell.Format.Font = new Font { Name = FontsHelper.BebasNeueRegular, Size = 15, Color = ColorsHelper.White };
        cell.Shading.Color = ColorsHelper.Green900;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 7;
    }

    private static void AddHeaderForBillingAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Format.Font = new Font { Name = FontsHelper.BebasNeueRegular, Size = 15, Color = ColorsHelper.White };
        cell.Shading.Color = ColorsHelper.Green500;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private static void SetStyleBaseForBillingInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontsHelper.RobotoRegular, Size = 10, Color = ColorsHelper.Black };
        cell.Shading.Color = ColorsHelper.Green200;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private static void AddAmountForBilling(Cell cell, decimal amount)
    {
        cell.AddParagraph($"{CurrencySymbol} {amount}");
        cell.Format.Font = new Font{ Name = FontsHelper.RobotoRegular, Size = 10, Color = ColorsHelper.Black };
        cell.Shading.Color = ColorsHelper.White;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }
    
    private static void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

    private static byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,
        };
        
        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);
        
        return file.ToArray();
    }
}