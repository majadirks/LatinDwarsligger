using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LatinDwarsliggerLogic;

namespace LatinDwarsliggerTest;

[TestClass]
public class PdfAdapterTest
{
    [TestMethod]
    public void GetNumberOfPages_Test()
    {
        PdfAdapter.GeneratePdf("resources/Tacitus.pdf");
    }
}
