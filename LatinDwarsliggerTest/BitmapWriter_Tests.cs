using LatinDwarsliggerLogic;

namespace LatinDwarsliggerTest;

[TestClass]
public class BitmapWriter_Tests
{
    [TestMethod]
    public void WriteAeneidToBitmapTest()
    {
        // ToDo: Make column explicit (not read from file),
        // add assertions

        // Arrange
        string path = "resources/aen1.html";
        var paragraphs = HtmlCleaner.FormatHtmlFile(path);
        Arranger arr = new(fontFamilyName: "Arial", 
            emSizePoints: 11, 
            pageDoubleHeightInches: 8.5m, 
            pageWidthInches: 8.5m, 
            leftRightMarginInches: 0.2m, 
            topBottomMarginInches: 0.2m, 
            pixelsPerInch: 320);
        var columns = arr.ArrangeParagraphsIntoColumns(paragraphs);

        // Act
        foreach (var column in columns)
        {
            column.ToBitmap();
        }

    }
}
