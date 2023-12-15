using LatinDwarsliggerLogic;
namespace LatinDwarsliggerTest;
#pragma warning disable CA1416 // Validate platform compatibility

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

    [TestMethod]
    public void WriteAeneidToPageBitmaps_Test()
    {
        // ToDo: Make explicit, add assertions

        // Arrange
        string path = "resources/aen1.html";
        var paragraphs = HtmlCleaner.FormatHtmlFile(path);
        Arranger arr = Arranger.Default;
        var columns = arr.ArrangeParagraphsIntoColumns(paragraphs);
        var halfSides = arr.ArrangeColumnsIntoHalfSides(columns);
        var pages = arr.ArrangeHalfSidesIntoPaperSheets(halfSides);

        // Act
        var images = pages.Select(page => page.ToBitmaps(arr)).ToArray();

        for (int i = 0; i < images.Length; i++)
        {
            var image = images[i];
            image.SideASideD.Save($"{i:D2}_sideAsideD.bmp");
            image.SideBSideC?.Save($"{i:D2}_sideBsideC.bmp");
        }
    }
}
#pragma warning restore CA1416 // Validate platform compatibility
