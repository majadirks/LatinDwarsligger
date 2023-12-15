using LatinDwarsliggerLogic;

namespace LatinDwarsliggerTest;

[TestClass]
public class BitmapWriter_Tests
{
    [TestMethod]
    public void WriteAeneidToBitmap_Columns_Test()
    {
        // ToDo: Make column explicit (not read from file),
        // add assertions

        // Arrange
        string path = "resources/aen1.html";
        var paragraphs = HtmlCleaner.FormatHtmlFile(path);
        Arranger arr = Arranger.Default;
        var columns = arr.ArrangeParagraphsIntoColumns(paragraphs);

        // Act
        foreach (var column in columns)
        {
            column.ToBitmap();
        }
    }

    [TestMethod]
    public void WriteAeneidToBitmap_HalfPages_Test()
    {
        // ToDo: Make column explicit (not read from file),
        // add assertions

        // Arrange
        string path = "resources/aen1.html";
        var paragraphs = HtmlCleaner.FormatHtmlFile(path);
        Arranger arr = Arranger.Default;
        var columns = arr.ArrangeParagraphsIntoColumns(paragraphs);
        var halfSides = arr.ArrangeColumnsIntoHalfSides(columns);

        // Act
        foreach (var halfSide in halfSides)
        {
            halfSide.ToBitmap(arr);
        }
    }
}
