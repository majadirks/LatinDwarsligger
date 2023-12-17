// Run this tool from the command line by supp

using LatinDwarsliggerLogic;

if (args.Length == 0 || !args.Where(arg => arg.Contains("thelatinlibrary.com")).Any())
{
    Console.WriteLine("Please provide the URL of a Latin Library text.");
    Console.WriteLine("For example:");
    Console.WriteLine("\t.\\MakeDwarsligger.exe \"https://www.thelatinlibrary.com/valeriusflaccus1.html\"");
    return;
}

var urls = args.Where(arg => arg.Contains("thelatinlibrary.com"));
if (urls.Count() > 1)
{
    Console.WriteLine("One url at a time, please!");
    return;
}

string url = args.Where(arg => arg.Contains("thelatinlibrary.com")).Single();

// name is everything between the last "/" and the file extension, with periods replaced by underscores
// eg "https://www.thelatinlibrary.com/carm.bur.html" has a name of "carm_bur"
string name = string.Join("_", url.Split("/").Last().Split(".").SkipLast(1));

Console.WriteLine("Parsing HTML...");
var paragraphs = await HtmlCleaner.FormatHtmlFromUrl(url);
Console.WriteLine($"Arranging {paragraphs.Count()} paragraphs into columns ...");
Arranger arr = Arranger.Default;
var columns = arr.ArrangeParagraphsIntoColumns(paragraphs);
Console.WriteLine($"Arranging {columns.Count()} columns into half-sides...");
var halfSides = arr.ArrangeColumnsIntoHalfSides(columns);
Console.WriteLine($"Arranging {halfSides.Count()} half-sides into sheets...");
var pages = arr.ArrangeHalfSidesIntoPaperSheets(halfSides).ToArray();

Console.WriteLine("Generating images...");

List<PaperSheetImages> psis = [];
for (int i = 0; i < pages.Length; i++)
{
    Console.WriteLine($"\tGenerating image {i + 1} of {pages.Length}...");
    PaperSheet page = pages[i];
    psis.Add(page.ToBitmaps(arr));
}

Console.WriteLine("Generating PDF...");
IProgress<string> logger = new Progress<string>(str => Console.WriteLine($"\t{str}"));
DwarsliggerPdf.GeneratePdf($"{name}.pdf", psis, logger);

Console.WriteLine("Done.");
