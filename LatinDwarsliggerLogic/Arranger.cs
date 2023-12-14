using System.Drawing;
using System.Diagnostics;
namespace LatinDwarsliggerLogic;

#pragma warning disable CA1416 // Validate platform compatibility
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
public class Arranger
{
    private const float TYPOGRAPHIC_POINTS_PER_INCH = 72;
    public Arranger(String fontFamilyName, float emSizePoints, decimal pageDoubleHeightInches, decimal pageWidthInches, decimal leftRightMarginInches, decimal topBottomMarginInches, int pixelsPerInch = 320)
    {
        float emSizeInches = emSizePoints / TYPOGRAPHIC_POINTS_PER_INCH;
        this.font = new Font(familyName: fontFamilyName, emSize: emSizeInches, style: FontStyle.Regular, unit: GraphicsUnit.Inch);
        PageDoubleHeightInches = pageDoubleHeightInches;
        PageWidthInches = pageWidthInches;
        LeftRightMarginInches = leftRightMarginInches;
        TopBottomMarginInches = topBottomMarginInches;      
        PixelsPerInch = pixelsPerInch;
    }

    private readonly Font font;
    public decimal PageDoubleHeightInches { get; init; }
    public decimal HalfSideHeightInches => (PageDoubleHeightInches / 2) - (TopBottomMarginInches * 2);
    public decimal PageWidthInches { get; init; }
    public decimal LeftRightMarginInches { get; init; }
    public decimal TopBottomMarginInches { get; init; }
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
        float lineHeight = font.Size;

        string[] lines = paragraphs
            .SelectMany(p => p.Lines.Append(" ")) // Add paragraph break after each paragraph
            .SkipLast(1) // ignore paragraph break
            .ToArray();

        int i = 0;
        bool lineAdded;
        for (int colIdx = 0; i < lines.Length; colIdx++)
        {
            lineAdded = false;
            Column col = new(
                font: font, 
                leftRightMarginInches: LeftRightMarginInches,
                topBottomMarginInches: TopBottomMarginInches, 
                maxHeightInches: HalfSideHeightInches, 
                maxWidthInches: PageWidthInches,
                pixelsPerInch: PixelsPerInch);
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
}
#pragma warning restore CA1416 // Validate platform compatibility