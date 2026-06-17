using System.Reflection;
using PdfSharp.Fonts;

namespace BarberBoss.Application.UseCases.Billings.Reports.Pdf.Fonts;

public class BillingsReportFontResolver : IFontResolver
{
    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName);
    }

    public byte[]? GetFont(string faceName)
    {
        var stream = ReadFontFile(faceName) ?? ReadFontFile(FontsHelper.DefaultFont);
        
        var length = (int)stream!.Length;
        var data = new byte[length];
        
        stream.ReadExactly(data, 0 , length);
        return data;
    }

    private static Stream? ReadFontFile(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream(
            $"BarberBoss.Application.UseCases.Billings.Reports.Pdf.Fonts.{faceName}.ttf"
        );
    }
}