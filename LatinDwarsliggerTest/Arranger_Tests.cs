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
    public void ArrangeAeneidColumnsIntoParagraphs()
    {
        // ToDo: Add assertions, make text more explicit in code rather than reading from file

        // Arrange
        string path = "resources/aen1.html";
        var paragraphs = HtmlCleaner.FormatHtmlFile(path);

        Font font = new Font(FontFamily.GenericSerif, emSize: 11, FontStyle.Regular, GraphicsUnit.Point);
        Arranger arr = new(font, 8.5m, 8.5m, 0.2m, 0.2m);

        // Act
        var actual = arr.ArrangeParagraphsIntoColumns(paragraphs);
    }

    [TestMethod]
    public void ArrangeAeneidParagraphsIntoHalfSides()
    {
        // ToDo: Make columns explicit (not read from file),
        // add assertions

        // Arrange
        string path = "resources/aen1.html";
        var paragraphs = HtmlCleaner.FormatHtmlFile(path);
        Font font = new Font(FontFamily.GenericSerif, emSize: 11, FontStyle.Regular, GraphicsUnit.Point);
        Arranger arr = new(font, 8.5m, 8.5m, 0.2m, 0.2m);
        var columns = arr.ArrangeParagraphsIntoColumns(paragraphs);

        // Act
        var halfSides = arr.ArrangeColumnsIntoHalfSides(columns);
    }

}
#pragma warning restore CA1416 // Validate platform compatibility