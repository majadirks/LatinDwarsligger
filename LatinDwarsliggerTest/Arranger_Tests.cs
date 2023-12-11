using LatinDwarsliggerLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinDwarsliggerTest;
#pragma warning disable CA1416 // Validate platform compatibility

[TestClass]
public class Arranger_Tests
{
    [TestMethod]
    public void ArrangeAeneid()
    {
        // Arrange
        string path = "resources/aen1.html";
        var paragraphs = HtmlCleaner.FormatHtmlFile(path);

        Graphics g = Graphics.FromHwnd(IntPtr.Zero);

        Font font = new Font(FontFamily.GenericSerif, emSize: 11, FontStyle.Regular, GraphicsUnit.Point);
        Arranger arr = new(g, font, 8.5m, 8.5m, 0.2m, 0.2m);

        // Act
        var actual = arr.ArrangeIntoColumns(paragraphs);
    }

}
#pragma warning restore CA1416 // Validate platform compatibility