using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iTextImage = iText.Layout.Element.Image;

namespace LatinDwarsliggerLogic;



public static class PdfAdapter
{
    /* ToDo: read pdf, take pages 12 at a time, lay out like this:
     * 
     * 10 11 12     |   6 8 ㄥ     (7/8/9 upside down)
     * 1  2  3      |   9 ϛ h     (4/5/6 upside down)
     * 
     * generate bitmaps, save to new pdf
     * 
    */

    public static iText.Layout.Document GeneratePdf(string inputPath, string outputPath)
    {
        FileInfo file = new(inputPath);
        PdfReader reader = new(file);
        PdfDocument pdfDoc = new(reader);
        int inputPages = pdfDoc.GetNumberOfPages();

        PdfWriter writer = new(outputPath);
        PdfDocument output = new(writer);
        
        for (int i = 0; i < inputPages - 12; i += 12)
        {
            // first 12 pages: i = 0..11
            // next 12 pages: i = 12..23
            PdfPage[] pages = 
                Enumerable.Range(i, count: 12)
                .Select(j => pdfDoc.GetPage(j))
                .ToArray();

        }

        throw new NotImplementedException();

    }


}
