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

        [TestMethod]
        public void RemoveRedundantParagraphTags_Test()
        {
            // Arrange
            string[] input =
                [
                    @"<p>",
                    @"<p>",
                    @"Arma virumque canō, Trōiae quī prīmus ab ōrīs",
                    @"Ītaliam, fātō profugus, Lāvīniaque vēnit",
                    @"<p>",
                    @"Mūsa, mihī causās memorā, quō nūmine laesō,",
                    @"quidve dolēns, rēgīna deum tot volvere cāsūs",
                    @"<p>",
                    @"<p>",
                    @"<p>",
                    @"Urbs antīqua fuit, Tyriī tenuēre colōnī,"
                ];

            string[] expected =
                [
                    @"<p>", // fix duplicate
                    @"Arma virumque canō, Trōiae quī prīmus ab ōrīs",
                    @"Ītaliam, fātō profugus, Lāvīniaque vēnit",
                    @"<p>",
                    @"Mūsa, mihī causās memorā, quō nūmine laesō,",
                    @"quidve dolēns, rēgīna deum tot volvere cāsūs",
                    @"<p>", // fix triplicate
                    @"Urbs antīqua fuit, Tyriī tenuēre colōnī,"
                ];

            // Act
            string[] actual = input.RemoveRedundantParagraphTags().ToArray();

            // Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }


        [TestMethod]
        public void FormatAeneid()
        {
            // arrange
            string rawHtml = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD HTML 4.01//EN"" ""http://www.w3.org/TR/html4/strict.dtd"">
<html><head>
		<title>
			Vergil: Aeneid I
		</title>

		<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
		
		<link rel=""shortcut icon"" href=""http://www.thelatinlibrary.com/icon.ico"">
		
		<link rel=""StyleSheet"" href=""http://www.thelatinlibrary.com/latinlibrary.css"">
		<link rel=""StyleSheet"" media=""print"" href=""http://www.thelatinlibrary.com/latinlibrary_print.css"">
	<link href=""data:text/css,%3Ais(%5Bid*%3D'google_ads_iframe'%5D%2C%5Bid*%3D'taboola-'%5D%2C.taboolaHeight%2C.taboola-placeholder%2C%23credential_picker_container%2C%23credentials-picker-container%2C%23credential_picker_iframe%2C%5Bid*%3D'google-one-tap-iframe'%5D%2C%23google-one-tap-popup-container%2C.google-one-tap-modal-div%2C%23amp_floatingAdDiv%2C%23ez-content-blocker-container)%20%7Bdisplay%3Anone!important%3Bmin-height%3A0!important%3Bheight%3A0!important%3B%7D"" rel=""stylesheet"" type=""text/css""></head>
	
<body>


<h1>P. VERGILI MARONIS AENEIDOS LIBER PRIMVS</h1>


<p class=""internal_navigation"">

</p><p>
Arma virumque canō, Trōiae quī prīmus ab ōrīs<br>
Ītaliam, fātō profugus, Lāvīniaque vēnit<br>
lītora, multum ille et terrīs iactātus et altō<br>
vī superum saevae memorem Iūnōnis ob īram;<br>
multa quoque et bellō passus, dum conderet 
urbem,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">5</span><br>
inferretque deōs Latiō, genus unde Latīnum,<br>
Albānīque patrēs, atque altae moenia Rōmae.
</p>

<p class=""poem"">
Mūsa, mihī causās memorā, quō nūmine laesō,<br>
quidve dolēns, rēgīna deum tot volvere cāsūs<br>
īnsīgnem pietāte virum, tot adīre 
labōrēs&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">10</span><br>
impulerit. Tantaene animīs caelestibus īrae?
</p>

<div class=""footer"">
	<p>
		<a href=""https://www.thelatinlibrary.com/verg.html"">Vergil</a>
		<a href=""https://www.thelatinlibrary.com/index.html"">The Latin Library</a>
		<a href=""https://www.thelatinlibrary.com/classics.html"">The Classics Page</a>
	</p>
</div>

</body></html>";
            string[] rawHtmlLines = rawHtml.Split(Environment.NewLine);
            ChunkOfText[] expected =
                [
                    new(["Arma virumque canō, Trōiae quī prīmus ab ōrīs",
                    "Ītaliam, fātō profugus, Lāvīniaque vēnit",
                    "lītora, multum ille et terrīs iactātus et altō",
                    "vī superum saevae memorem Iūnōnis ob īram;",
                    "multa quoque et bellō passus, dum conderet urbem,",
                    "inferretque deōs Latiō, genus unde Latīnum,",
                    "Albānīque patrēs, atque altae moenia Rōmae."]),
                    new(["Mūsa, mihī causās memorā, quō nūmine laesō,",
                    "quidve dolēns, rēgīna deum tot volvere cāsūs",
                    "īnsīgnem pietāte virum, tot adīre labōrēs",
                    "impulerit. Tantaene animīs caelestibus īrae?"])
                ];

            // Act
            var actual = HtmlFormatter.FormatHtmlCode(rawHtmlLines).ToArray();

            // Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                ChunkOfText expectedCot = expected[i];
                ChunkOfText actualCot = actual[i];
                Assert.AreEqual(expectedCot, actualCot);
            }
        }
    }
}