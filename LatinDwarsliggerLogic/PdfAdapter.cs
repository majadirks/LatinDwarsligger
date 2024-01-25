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

    public static iText.Layout.Document GeneratePdf(string inputFile)
    {
        FileInfo file = new FileInfo(inputFile);
        PdfReader reader = new PdfReader(file);
        PdfDocument pdfDoc = new PdfDocument(reader);
        int numberofpages = pdfDoc.GetNumberOfPages();
        Console.WriteLine(numberofpages);
        throw new NotImplementedException();
    }


}
