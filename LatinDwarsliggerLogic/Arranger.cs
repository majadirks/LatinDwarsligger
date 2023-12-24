using System.Drawing;
using System.Diagnostics;
using System.Text;
namespace LatinDwarsliggerLogic;

#pragma warning disable CA1416 // Validate platform compatibility

public delegate SizeF StringMeasurer(string text);

/// <summary>
/// Given some chunks of text,
/// decide how to lay them out on a page
///     /// Consecutive pages A, B, C, D should come out looking like

/// -------  -------    
/// |  B  |	 | ∀  |
/// |-----|	 |-----|
/// |  C  |	 | (|  | (upside-down D)
/// ------	 -------
///
/// Two-column format where possible 
///   - If two chunks fit side-by-side on one HalfSide, make it so
///   - Similarly, if the font size allows two columns of dactylic hexameter
///   - to fit on a HalfSide, make it so.
/// </summary>
public class Arranger : IDisposable
{
    public static Arranger Default = new (
            fontFamilyName:"Arial", 
            emSizePoints: 11, 
            pageDoubleHeightInches: 8.5f, 
            pageWidthInches: 8.5f, 
            leftRightMarginInches: 0.2f, 
            topBottomMarginInches: 0.2f,
            pixelsPerInch: 320);
    private const float TYPOGRAPHIC_POINTS_PER_INCH = 72;
    public Arranger(string fontFamilyName, float emSizePoints, float pageDoubleHeightInches, float pageWidthInches, float leftRightMarginInches, float topBottomMarginInches, int pixelsPerInch)
    {
        disposed = false;
        FontSizePoints = emSizePoints;
        float emSizeInches = emSizePoints / TYPOGRAPHIC_POINTS_PER_INCH;
        PageDoubleHeightInches = pageDoubleHeightInches;
        PageWidthInches = pageWidthInches;
        LeftRightMarginInches = leftRightMarginInches;
        TopBottomMarginInches = topBottomMarginInches;
        PixelsPerInch = pixelsPerInch;

        this.Font = new Font(familyName: fontFamilyName, emSize: emSizeInches, style: FontStyle.Regular, unit: GraphicsUnit.Inch);
        this.bitmap = new(width: Convert.ToInt32(PageWidthInches * pixelsPerInch), height: Convert.ToInt32(HalfSideHeightInches * pixelsPerInch));
        this.graphics = Graphics.FromImage(bitmap);
        graphics.PageUnit = GraphicsUnit.Inch;
        measureString = line => graphics.MeasureString(text: line, font: Font);
    }

    public float FontSizePoints { get; init; }
    private readonly Bitmap bitmap;
    private readonly Graphics graphics;
    private readonly StringMeasurer measureString;
    private bool disposed;
    public Font Font { get; init; }
    public float PageDoubleHeightInches { get; init; }
    public float HalfSideHeightInches => (PageDoubleHeightInches / 2) - (TopBottomMarginInches * 2);
    public float PageWidthInches { get; init; }
    public float LeftRightMarginInches { get; init; }
    public float TopBottomMarginInches { get; init; }
    public int PixelsPerInch { get; init; }

    public IEnumerable<Column> ArrangeParagraphsIntoColumns(IEnumerable<Paragraph> paragraphs)
    {
        var columns = new List<Column>();
        // Figure out what will fit in column A1.
        // Then figure out what will go in "the next column".
        // If the two columns can fit side-by-side, call both HalfPageA.
        // Otherwise, add the first column to Half Page A
        // and assign thesecond column to column B1.
        // Proceed until we have filled up through D1 (or D2 if possible).

        // Or: Find the first eight columns. Assign them to spots A1-D2 as possible.


        float halfSideHeightInches = Convert.ToSingle(HalfSideHeightInches);
        float lineHeight = Font.Size;

        string[] lines = paragraphs
            .SelectMany(p => p.Lines.Append(" ")) // Add paragraph break after each paragraph
            .SelectMany(BreakLineAtPage) // make sure lines don't exceed max allowable width
            .SkipLast(1) // ignore paragraph break
            .ToArray();

        int i = 0;
        bool lineAdded;
        for (int colIdx = 0; i < lines.Length; colIdx++)
        {
            lineAdded = false;
            Column col = new(font: Font, pixelsPerInch: PixelsPerInch, measureString: measureString);
            // In new column, skip any opening breaks
            string line = lines[i];
            while (string.IsNullOrWhiteSpace(line) && i < lines.Length)
            {
                i++;
                line = lines[i];
            }

            // Add lines until the next line would push the column above the max height
            for (; col.HeightInInches() + lineHeight < halfSideHeightInches && i < lines.Length; i++)
            {
                line = lines[i];
                col.Contents.Add(line);
                lineAdded = true;
            }

            if (!lineAdded)
                throw new Exception($"Could not add line '{line}' to a column.");

            // If final two lines are a break and a line, move the line to the next column
            if (col.Contents.Count >= 2 && string.IsNullOrWhiteSpace(col.Contents.SkipLast(1).Last()))
            {
                i--;
                col.RemoveFinalTwoLines();
            }

            columns.Add(col);
        }
        return columns;
    }

    public IEnumerable<HalfSide> ArrangeColumnsIntoHalfSides(IEnumerable<Column> columns)
    {
        var halfSides = new List<HalfSide>(capacity: 1 + (columns.Count() / 2));
        var columnsQ = new Queue<Column>(columns);
        float pageWidthInches = Convert.ToSingle(PageWidthInches);
        while (columnsQ.Count > 0)
        {
            Column col1 = columnsQ.Dequeue();
            bool col2Exists = columnsQ.TryPeek(out Column? col2);
            if (col2Exists)
            {
                var WithBoth = new HalfSide(col1, col2);
                if (WithBoth.Width < pageWidthInches)
                {
                    columnsQ.Dequeue();
                    halfSides.Add(WithBoth);
                }
                else
                {
                    halfSides.Add(new(col1));
                }
            }
            else // no second column remains
            {
                halfSides.Add(new(col1));
            }
            
        }
        return halfSides;
    }

    public IEnumerable<PaperSheet> ArrangeHalfSidesIntoPaperSheets(IEnumerable<HalfSide> halfSides)
    {
        var paperSheets = new List<PaperSheet>();
        var sidesQ = new Queue<HalfSide>(halfSides);
        while (sidesQ.Count > 0)
        {
            HalfSide?[] sidesOnSheet = new HalfSide?[4];
            var sideA = sidesQ.Dequeue(); // side A
            sidesOnSheet[0] = sideA;
            Debug.Assert(sideA != null);
            for (int i = 1; i <= 3 && sidesQ.TryDequeue(out HalfSide? next); i++)
            {
                sidesOnSheet[i] = next; // sides B, C, and D, if they exist
            }
            var nextSheet = new PaperSheet(sideA, sidesOnSheet[1], sidesOnSheet[2], sidesOnSheet[3]);
            paperSheets.Add(nextSheet);
        }
        return paperSheets;
    }

    private IEnumerable<string> BreakLineAtPage(string line)
    {
        float maxWidth = PageWidthInches - 2 * LeftRightMarginInches;
        if (measureString(line).Width < maxWidth)
            return new string[] { line };

        Queue<string> words = new(line.Split(" "));
        List<string> broken = [];

        StringBuilder nextLine = new();
        while (words.Count > 0)
        {
            while (measureString(nextLine.ToString()).Width < maxWidth && words.TryDequeue(out string? nextWord))
            {
                float nextWordWidth = measureString(nextWord).Width;
                if (nextWordWidth >= maxWidth)
                {
                    // Unusual case: the word is wider than a page!
                    broken.AddRange(CurrentLinePlusBrokenLongWord(nextLine, nextWord));
                    nextLine = new();
                    continue; // return to next iteration of inner while loop
                }
                else
                {
                    // Normal case: the next word is not unusually long
                    nextLine.Append(nextWord + " ");
                }
            }
            if (nextLine.Length > 0) 
                broken.Add(nextLine.ToString());
            nextLine = new();
        }
        return broken;
    }

    private IEnumerable<string> CurrentLinePlusBrokenLongWord(StringBuilder nextLine, string longWord)
    {
        List<string> toAdd = [];
        if (nextLine.Length > 0)
            toAdd.Add(nextLine.ToString());
        toAdd.AddRange(BreakLongWordAtPage(longWord));
        return toAdd;
    }

    /// <summary>
    /// In the rare case that we have a word that is wider than the page, break it up when it reaches the max
    /// </summary>
    /// <param name="longWord"></param>
    /// <returns></returns>
    private IEnumerable<string> BreakLongWordAtPage(string longWord)
    {
        float maxWidth = PageWidthInches - 2 * LeftRightMarginInches;
        Queue<char> chars = new(longWord);
        List<string> broken = [];
        StringBuilder nextLine = new();
        while (chars.Count > 0)
        {
            while (measureString(nextLine.ToString()).Width < maxWidth && chars.TryDequeue(out char nextChar))
            {
                nextLine.Append(nextChar);
            }
            broken.Add(nextLine.ToString());
            nextLine = new();
        }
        return broken;
    }

    public void Dispose()
    {
        if (disposed) return;
        graphics?.Dispose();
        bitmap?.Dispose();
        Font?.Dispose();
        GC.SuppressFinalize(this);
        disposed = true;
    }
    ~Arranger() => Dispose();
}
#pragma warning restore CA1416 // Validate platform compatibility