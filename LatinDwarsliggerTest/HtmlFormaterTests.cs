using LatinDwarsliggerLogic;

namespace LatinDwarsliggerTest
{
    [TestClass]
    public class HtmlFormaterTests
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
    }
}