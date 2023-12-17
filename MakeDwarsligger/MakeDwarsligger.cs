#pragma warning disable CA1416 // Validate platform compatibility
using LatinDwarsliggerLogic;

if (args.Length == 0 && !args.Where(arg => arg.Contains("thelatinlibrary.com")).Any())
{
    Console.WriteLine("Please provide the URL of a Latin Library text.");
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

HttpClient client = new HttpClient();
Console.WriteLine("Fetching page...");
string html = await client.GetStringAsync(url);
Console.WriteLine("Parsing HTML...");
var paragraphs = HtmlCleaner.FormatHtmlCode([html]);
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

Console.WriteLine($"Saving {psis.Count} images...");
for (int i = 0; i < psis.Count; i++)
{
    Console.WriteLine($"\tSaving page {i + 1} of {psis.Count}...");
    var image = psis[i];
    string pathAD = $"{name}_{i:D2}_sideAsideD.bmp";
    Console.WriteLine($"\t\t{pathAD}");
    image.SideASideD.Save(pathAD);
    string pathBC = $"{name}_{i:D2}_sideBsideC.bmp";
    Console.WriteLine($"\t\t{pathBC}");
    image.SideBSideC?.Save($"{i:D2}_sideBsideC.bmp");
}
Console.WriteLine("Done.");
#pragma warning restore CA1416 // Validate platform compatibility
