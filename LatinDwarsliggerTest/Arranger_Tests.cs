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
    public void ArrangeAeneidParagraphsIntoColumns()
    {
        // ToDo: Add assertions, make text more explicit in code rather than reading from file

        // Arrange
        string path = "resources/aen1.html";
        var paragraphs = HtmlCleaner.FormatHtmlFile(path);

        Font font = new Font(FontFamily.GenericSerif, emSize: 0.3f, FontStyle.Regular, GraphicsUnit.Inch);
        Arranger arr = Arranger.Default;

        // Act
        var actual = arr.ArrangeParagraphsIntoColumns(paragraphs);
    }

    [TestMethod]
    public void ArrangeAeneidColumnsIntoHalfSides()
    {
        // ToDo: Make columns explicit (not read from file),
        // add assertions

        // Arrange
        string path = "resources/aen1.html";
        var paragraphs = HtmlCleaner.FormatHtmlFile(path);
        Arranger arr = Arranger.Default;
        var columns = arr.ArrangeParagraphsIntoColumns(paragraphs);

        // Act
        var halfSides = arr.ArrangeColumnsIntoHalfSides(columns);
    }

    [TestMethod]
    public void ArrangeAeneidHalfSidesIntoPages()
    {
        // ToDo: Make half sides explicit (not read from file),
        // add assertions

        // Arrange
        string path = "resources/aen1.html";
        var paragraphs = HtmlCleaner.FormatHtmlFile(path);
        Arranger arr = Arranger.Default;
        var columns = arr.ArrangeParagraphsIntoColumns(paragraphs);
        var halfSides = arr.ArrangeColumnsIntoHalfSides(columns);

        // Act
        var pages = arr.ArrangeHalfSidesIntoPaperSheets(halfSides);
    }

    [TestMethod]
    public void ArrangeApuleiusIntoPages()
    {
        // Arrange
        string path = "resources/ApuleiusMetamorphosesI.htm";
        var paragraphs = HtmlCleaner.FormatHtmlFile(path);
        Arranger arr = Arranger.Default;
        var columns = arr.ArrangeParagraphsIntoColumns(paragraphs);
        var halfSides = arr.ArrangeColumnsIntoHalfSides(columns);

        // Act
        var pages = arr.ArrangeHalfSidesIntoPaperSheets(halfSides);
    }

    [TestMethod]
    public void BreakLongWordAtChar()
    {
        // Arrange
        string longString = "Quae res in civitate " + "duae plurimum possunt, eae contra nos ambae faciunt in hoc tempore, summa gratia et eloquentia; quarum alterum, C. Aquili, vereor, alteram metuo. Eloquentia Q. Hortensi ne me in dicendo impediat, non nihil commoveor, gratia Sex. Naevi ne P. Quinctio noceat, id vero non mediocriter pertimesco. Neque hoc tanto opere querendum videretur, haec summa in illis esse, si in nobis essent saltem mediocria; verum ita se res habet, ut ego, qui neque usu satis et ingenio parum possum, cum patrono disertissimo comparer, P. Quinctius, cui tenues opes, nullae facultates, exiguae amicorum copiae sunt, cum adversario gratiosissimo contendat.".Replace(" ", "");
        Paragraph paragraph = new Paragraph(new List<string> { longString });
        Arranger arr = Arranger.Default;

        // Act
        var columns = arr.ArrangeParagraphsIntoColumns(new List<Paragraph> { paragraph } );

        // Assert
        var contents = columns.Single().Contents;
        Assert.AreEqual("Quae res in civitate",contents.First().Trim());
        Assert.IsTrue(contents.Skip(1).First().StartsWith("duaeplurimumpossunt"));
        Assert.IsTrue(contents.Skip(2).Any());
    }

}
#pragma warning restore CA1416 // Validate platform compatibility