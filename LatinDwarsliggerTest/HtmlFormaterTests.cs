using LatinDwarsliggerLogic;

namespace LatinDwarsliggerTest
{
    [TestClass]
    public class HtmlFormaterTests
    {
        [TestMethod]
        public void Test_ReadFile()
        {
            // Arrange
            string path = @"resources\aen1.html";
            //Act
            string[] lines = File.ReadAllLines(path);

            // Assert
            Assert.IsNotNull(lines);
            Assert.IsTrue(lines.Any());
            Assert.IsTrue(lines.Any(str => str.Contains("rma virumque canō, Trōiae quī prīmus ab ōrīs")));
        }
    }
}