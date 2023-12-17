using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iTextImage = iText.Layout.Element.Image;
namespace LatinDwarsliggerLogic;

#pragma warning disable CA1416 // Validate platform compatibility
public static class DwarsliggerPdf
{
    public static Document GeneratePdf(string name, IEnumerable<PaperSheetImages> paperSheetImages, IProgress<string>? logger = null)
    {
        using var writer = new PdfWriter(name);
        using var pdfDoc = new PdfDocument(writer);
       
        string tempPath = "temp.bmp";
        paperSheetImages.First().SideASideD.Save(tempPath);
        iTextImage image = new(ImageDataFactory.Create(tempPath));
        using var document = new Document(pdfDoc, new PageSize(image.GetImageWidth(), image.GetImageHeight()));
        int i = 1;
        int max = paperSheetImages.Count();
        foreach (var psi in paperSheetImages)
        {
            logger?.Report($"Generating page {i} of {max}");
            AddSheet(document, pdfDoc, psi, tempPath);
            i++;
        }
        document.Close();

        if (File.Exists(tempPath))
            File.Delete(tempPath);

        return document;
    }

    private static void AddSheet(Document doc, PdfDocument pdfDoc, PaperSheetImages psi, string tempPath)
    {
        psi.SideASideD.Save(tempPath);
        iTextImage image = new(ImageDataFactory.Create(tempPath));
        pdfDoc.AddNewPage(new PageSize(image.GetImageWidth(), image.GetImageHeight()));
        doc.Add(image);

        if (psi.SideBSideC != null)
        {
            psi.SideBSideC.Save(tempPath);
            image = new(ImageDataFactory.Create(tempPath));
            pdfDoc.AddNewPage(new PageSize(image.GetImageWidth(), image.GetImageHeight()));
            doc.Add(image);
        }
    }

}
#pragma warning restore CA1416 // Validate platform compatibility