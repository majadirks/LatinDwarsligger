using LatinDwarsliggerLogic;

namespace LatinDwarsliggerTest
{
    [TestClass]
    public class HtmlFormatterTests
    {
        [TestMethod]
        public void StripLineNumbers_Test()
        {
            // Arrange
            string[] lines =
                [
                    @"</p><p>",
                    @"Arma virumque canō, Trōiae quī prīmus ab ōrīs<br>",
                    @"Ītaliam, fātō profugus, Lāvīniaque vēnit<br>",
                    @"lītora, multum ille et terrīs iactātus et altō<br>",
                    @"vī superum saevae memorem Iūnōnis ob īram;<br>",
                    @"multa quoque et bellō passus, dum conderet ",
                    @"urbem,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">5</span><br>",
                    @"inferretque deōs Latiō, genus unde Latīnum,<br>",
                    @"Albānīque patrēs, atque altae moenia Rōmae.",
                    @"</p>"
                ];

            string[] expected =
            {
                    @"</p><p>",
                    @"Arma virumque canō, Trōiae quī prīmus ab ōrīs<br>",
                    @"Ītaliam, fātō profugus, Lāvīniaque vēnit<br>",
                    @"lītora, multum ille et terrīs iactātus et altō<br>",
                    @"vī superum saevae memorem Iūnōnis ob īram;<br>",
                    @"multa quoque et bellō passus, dum conderet ",
                    @"urbem,<br>",
                    @"inferretque deōs Latiō, genus unde Latīnum,<br>",
                    @"Albānīque patrēs, atque altae moenia Rōmae.",
                    @"</p>"
            };

            // Act
            string[] actual = HtmlFormatter.StripLineNumbers(lines).ToArray();

            // Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void MoveParagraphBeginTagsToOwnLine_Test()
        {
            // Arrange
            string[] lines = [@"<p>Hello, World!</p>", @"<p>How are you?</p>"];
            string[] expected = [@"<p>", @"Hello, World!</p>", @"<p>", @"How are you?</p>"];
            // Act
            string[] actual = HtmlFormatter.MoveParagraphBeginTagsToOwnLine(lines).ToArray();
            // Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void MoveParagraphBeginTagsToOwnLine_MultipleInLine_Test()
        {
            // Arrange
            string[] lines = [@"<p>abc</p><p>def</p>"];
            string[] expected = [@"<p>", @"abc</p>", @"<p>", @"def</p>"];
            // Act
            string[] actual = HtmlFormatter.MoveParagraphBeginTagsToOwnLine(lines).ToArray();
            // Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void SplitOnBrTags_Test()
        {
            // Arrange
            string[] lines = 
                [
                @"</p><p>",
                @"This is on the first line,",
                @"as is this.<br>",
                @"This, however,",
                @"is the second line<br>",
                @"and this is the third.",
                @"</p>",
                @"<p>This is the fourth.</p>"
                ];
            string[] expected =
                [
                    @"<p>",
                    @"This is on the first line, as is this.",
                    @"This, however, is the second line",
                    @"and this is the third.",
                    @"<p>",
                    @"This is the fourth."
                ];
            // Act
            var actual = HtmlFormatter.SplitOnBrTags(lines).ToArray();
            // Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void StripTagAttributes_Test()
        {
            // Arrange
            string[] input = [@"<a href=""https://www.thelatinlibrary.com/vergil/aen1.shtml"">Aeneid</a><br><p class=""poem"">Aeneas scopulum interea conscendit</p>"];
            string[] expected = [@"<a>Aeneid</a><br><p>Aeneas scopulum interea conscendit</p>"];
            // Act
            string[] actual = input.StripTagAttributes().ToArray();
            //Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }
    }
}